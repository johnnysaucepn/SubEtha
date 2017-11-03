using System;
using System.Net.Http;
using Common.Logging;
using Howatworks.Configuration;
using Newtonsoft.Json;
using SubEtha.Domain;

namespace Thumb.Plugin.SubEtha
{
    public class HttpUploadClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger<HttpUploadClient>();

        private Uri BaseUri { get; }

        private readonly HttpClient _client;

        public HttpUploadClient(IConfigReader configReader)
        {
            _client = new HttpClient();
            
            BaseUri = new Uri(configReader.Get<string>("ServiceUri"));
        }

        public void Upload(Uri uri, IState state)
        {
            var targetUri = uri.IsAbsoluteUri ? uri : new Uri(BaseUri, uri);

            var response = _client.PostAsync(targetUri.AbsoluteUri, new StringContent(JsonConvert.SerializeObject(state))).Result;
            Log.Info($"HTTP {response.StatusCode}");
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