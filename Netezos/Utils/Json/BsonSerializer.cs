using System.IO;
using Newtonsoft.Json.Bson;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Netezos.Utils.Json
{
    public static class BsonSerializer
    {
        public static byte[] Serialize(object value)
        {
            using (var stream = new MemoryStream())
            using (var writer = new BsonDataWriter(stream))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(writer, value);
                return stream.ToArray();
            }
        }
    }
}