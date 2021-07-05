using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Rpc;

namespace Netezos.Utils.Json
{
    public class NormalizationConverter : JsonConverter<Normalization>
    {
        public override Normalization Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Normalization value, JsonSerializerOptions options)
        {
            var modeString = "";
            switch (value)
            {
                case Normalization.Readable:
                    modeString = "Readable";
                    break;
                case Normalization.Optimized:
                    modeString = "Optimized";
                    break;
                case Normalization.OptimizedLegacy:
                    modeString = "Optimized_legacy";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
            
            writer.WriteStringValue(modeString);
        }
    }
}