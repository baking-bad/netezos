using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace Netezos.Utils.Json
{
    public static class BsonSerializer
    {
        public static byte[] Serialize(string json)
        {
            var stream = new MemoryStream();
            using (var writer = new BsonDataWriter(stream))
            {
                var serializer = new JsonSerializer();
                var obj = JsonConvert.DeserializeObject(json);
                serializer.Serialize(writer, obj);
                return stream.ToArray();
            }
        }
        
        public static byte[] Serialize<T>(T value)
        {
            return Serialize(System.Text.Json.JsonSerializer.Serialize(value));
        }
    }
}