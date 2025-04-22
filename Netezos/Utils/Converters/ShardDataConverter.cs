using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class ShardDataConverter : JsonConverter<ShardData>
{
    public override ShardData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {

        var result = new ShardData();
        
        reader.Read();
        
        if (reader.TokenType == JsonTokenType.Number)
        {
            result.Id = reader.GetInt32();
        }
        else
        {
            throw new JsonException("Expected number as first element in ShardData");
        }
        
        reader.Read();
        
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected array as second element in ShardData");

        result.Hashes = [];
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                result.Hashes.Add(reader.GetString()!);
            }
        }

        reader.Read();
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, ShardData value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.Id);
        
        writer.WriteStartArray();
        foreach (var hash in value.Hashes)
        {
            writer.WriteStringValue(hash);
        }
        writer.WriteEndArray();
        
        writer.WriteEndArray();
    }
}