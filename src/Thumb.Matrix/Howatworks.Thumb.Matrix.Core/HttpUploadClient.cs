using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Howatworks.Matrix.Domain;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Howatworks.Thumb.Matrix.Core
{
    public class HttpUploadClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUploadClient));

        private readonly HttpClient _client;
        private readonly SortedList<DateTimeOffset, (Uri, IState)> _queue = new SortedList<DateTimeOffset, (Uri, IState)>();

        public Uri BaseUri { get; }
        public string SiteUri => BaseUri.AbsoluteUri;
       
        public bool IsAuthenticated { get; private set; }

        public HttpUploadClient(IConfiguration config)
        {
            BaseUri = new Uri(config["ServiceUri"]);
            _client = new HttpClient();
        }

        public void Push(Uri uri, IState state)
        {
            _queue.Add(state.TimeStamp, (uri, state));
        }

        public IObservable<DateTimeOffset> StartUploading(CancellationToken token)
        {
            return Observable.Create<DateTimeOffset>(o =>
                {
                    lock (_queue)
                    {
                        while (_queue.Count > 0 && !token.IsCancellationRequested)
                        {
                            var element = _queue.First();
                            var timestamp = element.Key;
                            (var uri, var state) = element.Value;
                            try
                            {
                                Upload(uri, state);
                                _queue.RemoveAt(0);
                                o.OnNext(timestamp);
                            }
                            catch (Exception ex)
                            {
                                o.OnError(ex);
                            }
                        }
                        o.OnCompleted();
                    }
                    return Disposable.Empty;
                });
        }

        public bool Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;

            AuthenticateByBearerToken(username, password);

            return IsAuthenticated;
        }

        public void AuthenticateByBearerToken(string username, string password)
        {
            try
            {
                var jwtTokenString = GetAuthToken(username, password).Result;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtTokenString);
                IsAuthenticated = true;
            }
            catch (AggregateException a)
            {
                a.Handle(innerEx =>
                {
                    Log.Error("Connection error", innerEx);
                    return true;
                });
            }
        }

        private async Task<string> GetAuthToken(string username, string password)
        {
            var tokenUri = new Uri(BaseUri, "Api/Token");
            var form = new Dictionary<string, string>
            {
                ["Username"] = username,
                ["Password"] = password
            };
            using (var tokenResponse = await _client.PostAsync(tokenUri, new FormUrlEncodedContent(form)).ConfigureAwait(false))
            {
                if (tokenResponse.IsSuccessStatusCode)
                {
                    return tokenResponse.Content.ReadAsStringAsync().Result;
                }
            }
            throw new MatrixAuthenticationException("Could not authenticate");
        }

        public void Upload(Uri uri, IState state)
        {
            var targetUri = uri.IsAbsoluteUri ? uri : new Uri(BaseUri, uri);

            if (!IsAuthenticated)
            {
                Log.Warn($"Not uploading to '{targetUri.AbsoluteUri}' as not authenticated");
                throw new MatrixAuthenticationException("Not authenticated");
            }
            try
            {
                Log.Info($"Uploading to '{targetUri.AbsoluteUri}'...");
                Log.Info(JsonConvert.SerializeObject(state));
                var response = _client.PostAsJsonAsync(targetUri.AbsoluteUri, state).Result;
                Log.Info($"HTTP {response.StatusCode}");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new MatrixAuthenticationException("Upload rejected - authentication failed");
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new MatrixUploadException(response.ReasonPhrase);
                }
            }
            catch (AggregateException a)
            {
                a.Handle(inner =>
                {
                    Log.Error("Connection error", inner);
                    return true;
                });
                throw new MatrixUploadException(a.Message);
            }
        }

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _client.Dispose();
            }

            _disposed = true;
        }
    }
}