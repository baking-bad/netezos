using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models;

public class ShardDataConverter : JsonConverter<ShardData>
{
    public override ShardData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Read();
        
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected number as first element in ShardData");
        
        var id = reader.GetInt32();
        reader.Read();
        
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected array as second element in ShardData");

        List<string> hashes = [];
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            hashes.Add(reader.GetString() ?? throw new Exception("Invalid hash string"));

        reader.Read();
        
        return new ShardData { Id = id, Hashes = hashes };
    }

    public override void Write(Utf8JsonWriter writer, ShardData value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        writer.WriteNumberValue(value.Id);
        
        writer.WriteStartArray();
        foreach (var hash in value.Hashes)
            writer.WriteStringValue(hash);
        writer.WriteEndArray();
        
        writer.WriteEndArray();
    }
}