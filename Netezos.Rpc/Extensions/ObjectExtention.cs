using Newtonsoft.Json;

namespace Netezos.Rpc
{
    static class ObjectExtention
    {
        public static string ToJson(this object obj) => JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
    }
}