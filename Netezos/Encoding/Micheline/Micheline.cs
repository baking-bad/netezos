using System.Text.Json;

namespace Netezos.Encoding
{
    public static class Micheline
    {
        static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { MaxDepth = 1024 };

        public static IMicheline FromJson(string json)
            => JsonSerializer.Deserialize<IMicheline>(json, SerializerOptions);

        public static string ToJson(IMicheline micheline)
            => JsonSerializer.Serialize(micheline, SerializerOptions);
    }
}
