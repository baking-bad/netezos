using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Netezos.Rpc
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content) : base(content)
        {
            Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        }
    }
}