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
        private readonly List<(Uri, IState)> _queue = new List<(Uri, IState)>();

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
            _queue.Add((uri, state));
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
                    (var uri, var state) = _queue.First();
                    var timestamp = state.TimeStamp;
                    try
                    {
                        await Upload(uri, state);
                        _queue.Remove((uri, state));
                        o.OnNext(timestamp);
                    }
                    catch (MatrixAuthenticationException ex)
                    {
                        o.OnError(ex);
                        break;
                    }
                    catch (MatrixUploadException ex)
                    {
                        // TODO: Possible too dramatic, all upload failures other than authentication treated as fatal
                        _queue.Remove((uri, state));
                        o.OnError(ex);
                        break;
                    }
                    catch (Exception ex)
                    {
                        o.OnError(ex);
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

            try
            {
                await AuthenticateByBearerToken(username, password);
            }
            catch (MatrixUploadException uex)
            {
                Log.Error(uex);
            }
            catch (MatrixAuthenticationException aex)
            {
                Log.Error(aex);
            }

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
                Log.Error(ex);
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