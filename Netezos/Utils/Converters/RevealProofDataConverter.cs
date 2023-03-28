using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class RevealProofDataConverter : JsonConverter<RevealProofData?>
    {
        public override RevealProofData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;

            sideReader.Read();
            while (!sideReader.ValueTextEquals("reveal_proof_kind"))
            {
                sideReader.Skip();
                sideReader.Read();
            }
            sideReader.Read();
            
            return sideReader.GetString() switch
            {
                "raw_data_proof" => JsonSerializer.Deserialize<RawDataProof>(ref reader, options),
                "metadata_proof" => JsonSerializer.Deserialize<MetadataProof>(ref reader, options),
                "dal_page_proof" => JsonSerializer.Deserialize<DalPageProof>(ref reader, options),
                _ => throw new JsonException("Invalid input proof kind"),
            };
        }

        public override void Write(Utf8JsonWriter writer, RevealProofData? value, JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}