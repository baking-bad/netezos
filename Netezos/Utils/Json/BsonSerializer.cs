using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Netezos.Utils.Json
{
    public class BsonSerializer
    {
        public static byte[] Serialize(string json)
        {
            var stream = new MemoryStream();
            using (var writer = new BsonDataWriter(stream))
            {
                JsonSerializer serializer = new JsonSerializer();
                var obj = JsonConvert.DeserializeObject(json);
                serializer.Serialize(writer, obj);
                return stream.ToArray();
            }
        }
    }
}