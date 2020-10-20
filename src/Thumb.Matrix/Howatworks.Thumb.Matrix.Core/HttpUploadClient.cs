﻿using System;
using System.Net.Http;
using log4net;
using Microsoft.Extensions.Configuration;
using Howatworks.Matrix.Domain;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Policy;

namespace Howatworks.Thumb.Matrix.Core
{
    public class HttpUploadClient : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpUploadClient));

        public string SiteUri => BaseUri.AbsoluteUri;

        public Uri BaseUri { get; }

        private readonly HttpClient _client;
        public bool IsAuthenticated { get; private set; }

        public HttpUploadClient(IConfiguration config)
        {
            BaseUri = new Uri(config["ServiceUri"]);
            _client = new HttpClient();
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