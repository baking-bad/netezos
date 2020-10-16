using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Encoding.Serialization
{
    public class MichelineConverter : JsonConverter<IMicheline>
    {
        public override IMicheline Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;

            if (sideReader.TokenType == JsonTokenType.StartArray)
                return JsonSerializer.Deserialize<MichelineArray>(ref reader, options);

            sideReader.Read();

            if (sideReader.ValueTextEquals("prim"))
                return JsonSerializer.Deserialize<MichelinePrim>(ref reader, options);

            reader.Read();
            IMicheline res;

            if (reader.ValueTextEquals("string"))
            {
                reader.Read();
                res = new MichelineString(reader.GetString());
            }
            else if (reader.ValueTextEquals("int"))
            {
                reader.Read();
                res = new MichelineInt(BigInteger.Parse(reader.GetString()));
            }
            else
            {
                reader.Read();
                res = new MichelineBytes(Hex.Parse(reader.GetString()));
            }

            reader.Read();
            return res;
        }

        public override void Write(Utf8JsonWriter writer, IMicheline value, JsonSerializerOptions options)
        {
            if (value is MichelineInt i)
            {
                writer.WriteStartObject();
                writer.WriteString("int", i.Value.ToString());
                writer.WriteEndObject();
            }
            else if (value is MichelineString s)
            {
                writer.WriteStartObject();
                writer.WriteString("string", s.Value);
                writer.WriteEndObject();
            }
            else if (value is MichelineBytes b)
            {
                writer.WriteStartObject();
                writer.WriteString("bytes", Hex.Convert(b.Value));
                writer.WriteEndObject();
            }
            else
            {
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
            }
        }
    }
}
