using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using log4net;
using Microsoft.Extensions.Configuration;
using Howatworks.Matrix.Domain;
using System.Collections.Generic;
using System.Threading;
using System.Net.Http.Headers;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class HttpUploadClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUploadClient));

        private Uri BaseUri { get; }

        private readonly HttpClient _client;

        private readonly Lazy<string> _jwtTokenString;

        public HttpUploadClient(IConfiguration config)
        {
            _client = new HttpClient();

            BaseUri = new Uri(config["Plugins:Howatworks.Thumb.Plugin.Matrix:ServiceUri"]);

            _jwtTokenString = new Lazy<string>(() =>
            {
                var tokenUri = new Uri(BaseUri, "Api/Token");
                var form = new Dictionary<string, string>
                {
                    ["Username"] = config["Plugins:Howatworks.Thumb.Plugin.Matrix:Username"],
                    ["Password"] = config["Plugins:Howatworks.Thumb.Plugin.Matrix:Password"]
                };
                var tokenResponse = _client.PostAsync(tokenUri, new FormUrlEncodedContent(form)).Result;
                if (tokenResponse.IsSuccessStatusCode)
                {
                    return tokenResponse.Content.ReadAsStringAsync().Result;
                }
                throw new InvalidOperationException("Could not authenticate");
            }, LazyThreadSafetyMode.PublicationOnly);

            var tokenString = _jwtTokenString.Value;
            if (_jwtTokenString.IsValueCreated)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtTokenString.Value);
            }
        }

        public void Upload(Uri uri, IState state)
        {
            var targetUri = uri.IsAbsoluteUri ? uri : new Uri(BaseUri, uri);

            try
            {
                Log.Info($"Uploading to {targetUri.AbsoluteUri}...");
                var response = _client.PostAsJsonAsync(targetUri.AbsoluteUri, state).Result;
                Log.Info($"HTTP {response.StatusCode}");
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