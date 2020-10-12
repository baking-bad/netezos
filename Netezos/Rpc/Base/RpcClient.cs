using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Dynamic.Json;
using Dynamic.Json.Extensions;

namespace Netezos.Rpc
{
    class RpcClient : IDisposable
    {
        #region static
        static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

        static readonly JsonSerializerOptions DefaultOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            MaxDepth = 1024,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };
        #endregion

        Uri BaseAddress { get; }
        TimeSpan RequestTimeout { get; }
        DateTime Expiration;

        HttpClient _HttpClient;
        protected HttpClient HttpClient
        {
            get
            {
                lock (this)
                {
                    // Workaround for https://github.com/dotnet/runtime/issues/18348
                    if (DateTime.UtcNow > Expiration)
                    {
                        _HttpClient?.Dispose();
                        _HttpClient = new HttpClient();

                        _HttpClient.BaseAddress = BaseAddress;
                        _HttpClient.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                        _HttpClient.DefaultRequestHeaders.UserAgent.Add(
                            new ProductInfoHeaderValue("Netezos", Version));
                        _HttpClient.Timeout = RequestTimeout;

                        Expiration = DateTime.UtcNow.AddMinutes(60);
                    }
                }

                return _HttpClient;
            }
        }

        public RpcClient(string baseUri, int timeoutSec = 30)
        {
            if (string.IsNullOrEmpty(baseUri))
                throw new ArgumentNullException(nameof(baseUri));

            if (!Uri.IsWellFormedUriString(baseUri, UriKind.Absolute))
                throw new ArgumentException("Invalid URI");

            BaseAddress = new Uri($"{baseUri.TrimEnd('/')}/");
            RequestTimeout = TimeSpan.FromSeconds(timeoutSec);
        }

        public RpcClient(HttpClient client)
        {
            _HttpClient = client ?? throw new ArgumentNullException(nameof(client));
            _HttpClient.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("Netezos", Version));

            Expiration = DateTime.MaxValue;
        }

        public Task<dynamic> GetJson(string path, CancellationToken cancellationToken = default)
        {
            return HttpClient.GetJsonAsync(path, DefaultOptions, cancellationToken);
        }

        public async Task<T> GetJson<T>(string path, CancellationToken cancellationToken = default)
        {
            using (var stream = await HttpClient.GetStreamAsync(path))
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, DefaultOptions, cancellationToken);
            }
        }

        public Task<dynamic> PostJson(string path, object data, CancellationToken cancellationToken = default)
        {
            var content = JsonSerializer.Serialize(data, DefaultOptions);
            return PostJson(path, content, cancellationToken);
        }

        public async Task<dynamic> PostJson(string path, string content, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.PostAsync(path, new JsonContent(content), cancellationToken);
            await EnsureResponceSuccessfull(response);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                return await DJson.ParseAsync(stream, DefaultOptions, cancellationToken);
            }
        }

        public Task<T> PostJson<T>(string path, object data, CancellationToken cancellationToken = default)
        {
            var content = JsonSerializer.Serialize(data, DefaultOptions);
            return PostJson<T>(path, content, cancellationToken);
        }

        public async Task<T> PostJson<T>(string path, string content, CancellationToken cancellationToken = default)
        {
            var response = await HttpClient.PostAsync(path, new JsonContent(content), cancellationToken);
            await EnsureResponceSuccessfull(response);

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                return await JsonSerializer.DeserializeAsync<T>(stream, DefaultOptions, cancellationToken);
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
                    : string.Empty;

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
