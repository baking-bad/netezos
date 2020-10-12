using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Utils;

namespace Netezos.Micheline.Serialization
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
                res = new MichelineString { Value = reader.GetString() };
            }
            else if (reader.ValueTextEquals("int"))
            {
                reader.Read();
                res = new MichelineInt { Value = int.Parse(reader.GetString()) };
            }
            else
            {
                reader.Read();
                res = new MichelineBytes { Value = Hex.Parse(reader.GetString()) };
            }

            reader.Read();
            return res;
        }

        public override void Write(Utf8JsonWriter writer, IMicheline value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
