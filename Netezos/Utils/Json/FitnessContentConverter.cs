using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Forging.Models;

namespace Netezos.Utils.Json
{
    public class FitnessContentConverter : JsonConverter<FitnessContent>
    {
        public override FitnessContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                var fitness = new FitnessContent();
                reader.Read();

                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    return new FitnessContent()
                    {
                        Major = "0",
                        Minor = "0"
                    };
                } 

                fitness.Major = reader.GetString();
                reader.Read();
                fitness.Minor = reader.GetString();
                reader.Read();
                return reader.TokenType == JsonTokenType.EndArray 
                    ? fitness 
                    : throw new JsonException("Too many values into fitness array");
            }

            throw new JsonException("Fitness is not array");
        }

        public override void Write(Utf8JsonWriter writer, FitnessContent value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteStringValue(value.Major);
            writer.WriteStringValue(value.Minor);
            writer.WriteEndArray();
        }
    }
}