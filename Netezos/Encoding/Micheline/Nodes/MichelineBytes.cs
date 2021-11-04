using System.IO;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineBytes : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Bytes;

        [JsonPropertyName("bytes")]
        [JsonConverter(typeof(HexConverter))]
        public byte[] Value { get; set; }

        public MichelineBytes(byte[] value) => Value = value;

        public void Write(BinaryWriter writer, int depth = 0)
        {
            var len = Value.Length;

            if (len >= 0x1F)
            {
                writer.Write((byte)((int)Type | 0x1F));
                writer.Write7BitInt(len);
            }
            else
            {
                writer.Write((byte)((int)Type | len));
            }

            writer.Write(Value);
        }
    }
}
