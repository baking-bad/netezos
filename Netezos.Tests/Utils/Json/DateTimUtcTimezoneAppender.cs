using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Tests.Forging
{
    // DateTime types are parsed without timezone but are assumed to be UTC.
    // So append 'Z' when writing JSON DateTime values to accurately represent UTC.
    class DateTimUtcTimezoneAppender : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString() ?? throw new Exception("Cannot deserialize DateTime from null"));
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    }
}
