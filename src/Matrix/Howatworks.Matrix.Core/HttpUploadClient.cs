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

namespace Howatworks.Matrix.Core
{
    public class HttpUploadClient : IDisposable
    {
        private class UploadItem
        {
            public const int MaxAttempts = 3;

            public Uri Uri { get; }
            public IState State { get; }
            public int Attempts { get; private set; }
            public bool Expired { get; private set; }

            public UploadItem(Uri uri, IState state)
            {
                Uri = uri;
                State = state;
            }

            public void FlagAttempt()
            {
                Attempts++;
                if (Attempts >= MaxAttempts)
                {
                    Expired = true;
                }
            }
        }

        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUploadClient));

        private readonly HttpClient _client;
        private readonly List<UploadItem> _queue = new List<UploadItem>();

        public Uri BaseUri { get; }
        public string SiteUri => BaseUri.AbsoluteUri;

        public bool IsAuthenticated { get; private set; }

        public readonly int MaxPasswordLength = 100;

        // Empirically-determined to match the default ASP.NET settings
        public readonly int MaxUsernameLength = 256;

        public HttpUploadClient(IConfiguration config)
        {
            BaseUri = new Uri(config["ServiceUri"]);
            _client = new HttpClient();
        }

        public void Push(Uri uri, IState state)
        {
            _queue.Add(new UploadItem(uri, state));
        }

        public IObservable<DateTimeOffset> StartUploading(CancellationToken token)
        {
            return Observable.Create<DateTimeOffset>(async o =>
            {
                if (_queue.Count > 0)
                {
                    Log.Debug($"Uploading {_queue.Count} items from queue...");
                }
                while (_queue.Count > 0 && !token.IsCancellationRequested)
                {
                    var item = _queue.First();
                    var timestamp = item.State.TimeStamp;
                    try
                    {
                        await Upload(item.Uri, item.State);
                        _queue.Remove(item);
                        o.OnNext(timestamp);
                    }
                    catch (MatrixUploadException ex)
                    {
                        // In case of bad data, don't re-attempt uploading this data
                        item.FlagAttempt();
                        if (item.Expired) _queue.Remove(item);

                        o.OnError(ex);
                        break;
                    }
                    catch (MatrixAuthenticationException ex)
                    {
                        o.OnError(ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        item.FlagAttempt();
                        if (item.Expired) _queue.Remove(item);

                        o.OnError(new MatrixUploadException("Unexpected error", ex));
                        break;
                    }
                }
                o.OnCompleted();
                return Disposable.Empty;
            });
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) return false;
            if (string.IsNullOrWhiteSpace(password)) return false;

            await AuthenticateByBearerToken(username, password);

            return IsAuthenticated;
        }

        public async Task AuthenticateByBearerToken(string username, string password)
        {
            try
            {
                var jwtTokenString = await GetAuthToken(username, password);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtTokenString);
                IsAuthenticated = true;
            }
            catch (Exception ex)
            {
                throw new MatrixUploadException("Connection error while authenticating", ex);
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
            try
            {
                using (var tokenResponse = await _client.PostAsync(tokenUri, new FormUrlEncodedContent(form)))
                {
                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        return await tokenResponse.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new MatrixAuthenticationException($"Could not authenticate - {tokenResponse.ReasonPhrase}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex.Message);
                throw new MatrixAuthenticationException("Could not authenticate", ex);
            }
        }

        public async Task Upload(Uri uri, IState state)
        {
            var targetUri = uri.IsAbsoluteUri ? uri : new Uri(BaseUri, uri);

            if (!IsAuthenticated)
            {
                Log.Warn($"Not uploading to '{targetUri.AbsoluteUri}' as not authenticated");
                throw new MatrixAuthenticationException("Not authenticated");
            }
            HttpResponseMessage response;
            try
            {
                Log.Info($"Uploading to '{targetUri.AbsoluteUri}'...");
                Log.Info(JsonConvert.SerializeObject(state));
                response = await _client.PostAsJsonAsync(targetUri.AbsoluteUri, state);
                Log.Info($"HTTP {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new MatrixUploadException("Connection error", ex);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new MatrixAuthenticationException($"Upload rejected - {response.ReasonPhrase}");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new MatrixUploadException(response.ReasonPhrase);
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