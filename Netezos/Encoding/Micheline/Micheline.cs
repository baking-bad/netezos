using System.Text.Json;

namespace Netezos.Encoding
{
    public static class Micheline
    {
        static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions().AddMichelineConverters();

        public static IMicheline FromJson(string json)
            => JsonSerializer.Deserialize<IMicheline>(json, SerializerOptions);

        public static string ToJson(IMicheline micheline)
            => JsonSerializer.Serialize(micheline, SerializerOptions);
    }
}
