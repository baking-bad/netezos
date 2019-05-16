using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc
{
    class RpcClient : IDisposable
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

            BaseAddress = new Uri(baseUri);
            RequestTimeout = TimeSpan.FromSeconds(timeoutSec);
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

        public void Dispose()
        {
            _HttpClient?.Dispose();
        }
    }
}
