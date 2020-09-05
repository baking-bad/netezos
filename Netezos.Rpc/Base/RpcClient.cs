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
    public class RpcClient : IDisposable
    {
        public Uri BaseAddress { get; }
        public TimeSpan RequestTimeout { get; }

        DateTime _Expiration;
        HttpClient _HttpClient;

        protected HttpClient HttpClient
        {
            get
            {
                lock (this)
                {
                    if (DateTime.UtcNow > _Expiration)
                    {
                        _HttpClient?.Dispose();
                        _HttpClient = new HttpClient();

                        _HttpClient.BaseAddress = BaseAddress;
                        _HttpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        _HttpClient.Timeout = RequestTimeout;

                        _Expiration = DateTime.UtcNow.AddMinutes(120);
                    }
                }

                return _HttpClient;
            }
        }

        public RpcClient(string baseUri, int timeoutSec = 10)
        {
            if (string.IsNullOrEmpty(baseUri))
                throw new ArgumentNullException(nameof(baseUri));

            if (!Uri.IsWellFormedUriString(baseUri, UriKind.Absolute))
                throw new ArgumentException("Invalid URI");

            BaseAddress = new Uri($"{baseUri.TrimEnd('/')}/");
            RequestTimeout = TimeSpan.FromSeconds(timeoutSec);
        }

        public RpcClient(HttpClient httpClient)
        {
            _HttpClient = httpClient;
            _Expiration = DateTime.MaxValue;
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
            _HttpClient?.Dispose();
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
