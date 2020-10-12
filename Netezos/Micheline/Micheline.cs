using System.Text.Json;
using Netezos.Micheline.Serialization;

namespace Netezos.Micheline
{
    public static class Micheline
    {
        static readonly JsonSerializerOptions SerializerOptions;

        static Micheline()
        {
            SerializerOptions = new JsonSerializerOptions();
            SerializerOptions.Converters.Add(new MichelineConverter());
            SerializerOptions.Converters.Add(new PrimTypeConverter());
        }

        public static IMicheline Parse(string json)
            => JsonSerializer.Deserialize<IMicheline>(json, SerializerOptions);
    }

    public interface IMicheline
    {
        MichelineNode Type { get; }
    }

    public enum MichelineNode
    {
        Int,
        Bytes,
        String,
        Array,
        Prim
    }
}
