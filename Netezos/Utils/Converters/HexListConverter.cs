using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models;

public class HexListConverter : JsonConverter<List<byte[]>>
{
    public override List<byte[]>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var hexList = JsonSerializer.Deserialize<List<string>>(ref reader, options);
        return hexList.Select(Hex.Parse).ToList();
    }

    public override void Write(Utf8JsonWriter writer, List<byte[]> value, JsonSerializerOptions options)
    {
        var hexList = value.Select(Hex.Convert);
        JsonSerializer.Serialize(writer, hexList, options);
    }
}