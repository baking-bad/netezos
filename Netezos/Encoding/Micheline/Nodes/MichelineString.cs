using System.IO;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineString : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.String;

        [JsonPropertyName("string")]
        public string Value { get; set; }

        public MichelineString(string value) => Value = value;

        public void Write(BinaryWriter writer)
        {
            var bytes = Utf8.Parse(Value);
            var len = bytes.Length;

            if (len >= 0x1F)
            {
                writer.Write((byte)((int)Type | 0x1F));
                writer.Write7BitInt(len);
            }
            else
            {
                writer.Write((byte)((int)Type | len));
            }

            writer.Write(bytes);
        }
    }
}
