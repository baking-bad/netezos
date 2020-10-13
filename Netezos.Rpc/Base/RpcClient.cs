using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc
{
    class RpcClient : IDisposable
    {
        public static readonly TimeSpan HTTP_CLIENT_LIFETIME = TimeSpan.FromHours(2);

        [Obsolete("Use HttpClient.BaseAddress instead.")]
        public Uri BaseAddress => _BaseUri;

        public TimeSpan RequestTimeout { get; }

        private HttpMessageHandler _Handler { get; }

        private Uri _BaseUri { get; }

        private DateTime _HttpClientCreated { get; set; }

        private HttpClient _HttpClient { get; set; }

        protected HttpClient HttpClient
        {
            get
            {
                lock (this)
                {
                    DateTime now = DateTime.UtcNow;
                    TimeSpan lifetime = now.Subtract(_HttpClientCreated);
                    if (lifetime > HTTP_CLIENT_LIFETIME)
                    {
                        _HttpClient.Dispose();
                        _HttpClient = CreateHttpClient(_Handler, _BaseUri, RequestTimeout);
                        _HttpClientCreated = now.Add(HTTP_CLIENT_LIFETIME);
                    }
                }

                return _HttpClient;
            }
        }

        [Obsolete("Use typed RpcClient(Uri, TimeSpan) instead.")]
        public RpcClient(string baseUri, int timeoutSec = 10)
            : this(new Uri(baseUri), TimeSpan.FromSeconds(timeoutSec))
        {
        }

        public RpcClient(Uri baseUri, TimeSpan requestTimeout)
            : this(new HttpClientHandler(), baseUri, requestTimeout)
        {
        }

        public RpcClient(HttpMessageHandler handler, Uri baseUri, TimeSpan requestTimeout)
        {
            if (handler is null)
                throw new ArgumentNullException(nameof(handler));

            if (baseUri is null)
                throw new ArgumentNullException(nameof(baseUri));

            RequestTimeout = requestTimeout;
            _Handler = handler;
            _BaseUri = baseUri;

            _HttpClient = CreateHttpClient(_Handler, _BaseUri, RequestTimeout);
            _HttpClientCreated = DateTime.UtcNow;
        }

        public async Task<JToken> GetJson(string path)
        {
            using (var stream = await HttpClient.GetStreamAsync(path))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return JToken.ReadFrom(jsonReader);
            }
        }

        public async Task<T> GetJson<T>(string path)
        {
            using (var stream = await HttpClient.GetStreamAsync(path))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(jsonReader);
            }
        }
                
        public async Task<JToken> PostJson(string path, string content)
        {
            var response = await HttpClient.PostAsync(path, new JsonContent(content));
            await EnsureResponceSuccessfull(response);

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return JToken.ReadFrom(jsonReader);
            }
        }  
        
        public async Task<T> PostJson<T>(string path, string content)
        {
            var response = await HttpClient.PostAsync(path, new JsonContent(content));
            await EnsureResponceSuccessfull(response);

            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        public void Dispose()
        {
            _HttpClient.Dispose();
        }

        private static HttpClient CreateHttpClient(HttpMessageHandler handler, Uri baseUri, TimeSpan requestTimeout)
        {
            HttpClient client = new HttpClient(handler)
            {
                BaseAddress = baseUri,
                Timeout = requestTimeout,
            };

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        private async Task EnsureResponceSuccessfull(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var message = response.Content.Headers.ContentLength > 0
                    ? await response.Content.ReadAsStringAsync()
                    : String.Empty;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new BadRequestException(message);
                    case HttpStatusCode.InternalServerError:
                        throw new InternalErrorException(message);
                    default:
                        throw new RpcException(response.StatusCode, message);
                }
            }
        }
    }
}
