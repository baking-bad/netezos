using System.Net.Http;
using System.Net.Http.Headers;

namespace Netezos.Rpc
{
    class JsonContent : StringContent
    {
        public JsonContent(string content) : base(content)
        {
            Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        }
    }
}