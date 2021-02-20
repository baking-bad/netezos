using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

namespace Netezos.Encoding
{
    public class MichelinePrim : IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Prim;

        [JsonPropertyName("prim")]
        public PrimType Prim { get; set; }

        [JsonPropertyName("args")]
        public List<IMicheline> Args { get; set; }

        [JsonPropertyName("annots")]
        public List<IAnnotation> Annots { get; set; }

        public void Write(BinaryWriter writer)
        {
            var argsCount = false;
            var annotsCount = false;

            var tag = (int)Type;
            if (Args?.Count > 0)
            {
                if (Args.Count >= 0x07)
                {
                    tag |= 0x70;
                    argsCount = true;
                }
                else
                {
                    tag |= Args.Count << 4;
                }
            }

            if (Annots?.Count > 0)
            {
                if (Annots.Count >= 0x0F)
                {
                    tag |= 0x0F;
                    annotsCount = true;
                }
                else
                {
                    tag |= Annots.Count;
                }
            }

            writer.Write((byte)tag);
            writer.Write((byte)Prim);

            if (Args != null)
            {
                if (argsCount)
                    writer.Write7BitInt(Args.Count);

                foreach (var arg in Args)
                    arg.Write(writer);
            }

            if (Annots != null)
            {
                if (annotsCount)
                    writer.Write7BitInt(Annots.Count);

                foreach (var annot in Annots)
                    WriteAnnotation(writer, annot);
            }
        }

        void WriteAnnotation(BinaryWriter writer, IAnnotation annot)
        {
            var bytes = Utf8.Parse(annot.Value);
            var len = bytes.Length;

            if (len >= 0x3F)
            {
                writer.Write((byte)((int)annot.Type | 0x3F));
                writer.Write7BitInt(len);
            }
            else
            {
                writer.Write((byte)((int)annot.Type | len));
            }

            writer.Write(bytes);
        }
    }
}
