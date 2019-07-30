using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using log4net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class HttpUploadClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUploadClient));

        private Uri BaseUri { get; }

        private readonly HttpClient _client;

        public HttpUploadClient(IConfiguration config)
        {
            _client = new HttpClient();

            BaseUri = new Uri(config["Plugins:Howatworks.Thumb.Plugin.Matrix:ServiceUri"]);
        }

        public void Upload(Uri uri, IState state)
        {
            var targetUri = uri.IsAbsoluteUri ? uri : new Uri(BaseUri, uri);

            try
            {
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