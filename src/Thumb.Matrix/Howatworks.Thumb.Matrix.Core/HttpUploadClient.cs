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

        private Uri BaseUri { get; }

        private readonly Lazy<HttpClient> _client;

        private string _jwtTokenString;

        public HttpUploadClient(IConfiguration config)
        {
            BaseUri = new Uri(config["ServiceUri"]);

            _client = new Lazy<HttpClient>(() =>
            {
                var client = new HttpClient();
                try
                {
                    _jwtTokenString = GetAuthToken(config, client).Result;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtTokenString);
                    return client;
                }
                catch (AggregateException a)
                {
                    a.Handle(inner =>
                    {
                        Log.Error("Connection error", inner);
                        if (inner is MatrixAuthenticationException ex)
                        {
                            throw ex;
                        }
                        return true;
                    });
                    throw;
                }
            });
        }

        private async Task<string> GetAuthToken(IConfiguration config, HttpClient client)
        {
            var tokenUri = new Uri(BaseUri, "Api/Token");
            var form = new Dictionary<string, string>
            {
                ["Username"] = config["Username"],
                ["Password"] = config["Password"]
            };
            using (var tokenResponse = await client.PostAsync(tokenUri, new FormUrlEncodedContent(form)).ConfigureAwait(false))
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

            try
            {
                Log.Info($"Uploading to {targetUri.AbsoluteUri}...");
                var response = _client.Value.PostAsJsonAsync(targetUri.AbsoluteUri, state).Result;
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
                if (_client.IsValueCreated)
                {
                    _client.Value.Dispose();
                }
            }

            _disposed = true;
        }
    }
}