﻿using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelineString(string value) : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.String;

        [JsonPropertyName("string")]
        public string Value { get; set; } = value;

        public void Write(BinaryWriter writer, int depth = 0)
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
