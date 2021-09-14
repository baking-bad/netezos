using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Netezos.Utils.Json
{
    public static class BsonSerializer
    {
        private static DefaultContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        public static byte[] Serialize(object value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BsonDataWriter(stream))
            {
                var serializer = new JsonSerializer
                {
                    ContractResolver = ContractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                };
                serializer.Serialize(writer, value);
                return stream.ToArray();
            }
        }
    }
}