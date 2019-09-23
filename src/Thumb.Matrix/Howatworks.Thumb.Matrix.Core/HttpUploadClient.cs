using System;
using System.Net.Http;
using log4net;
using Microsoft.Extensions.Configuration;
using Howatworks.Matrix.Domain;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Howatworks.Thumb.Matrix.Core
{
    public class HttpUploadClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUploadClient));

        public Uri BaseUri { get; }

        private readonly HttpClient _client;
        public bool IsAuthenticated { get; private set; }

        public HttpUploadClient(IConfiguration config)
        {
            BaseUri = new Uri(config["ServiceUri"]);
            _client = new HttpClient();
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
                return;
            }
            try
            {
                Log.Info($"Uploading to '{targetUri.AbsoluteUri}'...");
                var response = _client.PostAsJsonAsync(targetUri.AbsoluteUri, state).Result;
                Log.Info($"HTTP {response.StatusCode}");
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new MatrixAuthenticationException("Upload rejected - authentication failed");
                }
            }
            catch (AggregateException a)
            {
                a.Handle(inner =>
                {
                    Log.Error("Connection error", inner);
                    return true;
                });
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