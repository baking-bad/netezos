using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    public class MichelineArray : List<IMicheline>, IMicheline
    {
        [JsonIgnore]
        public MichelineType Type => MichelineType.Array;

        public MichelineArray() : base() { }
        public MichelineArray(int capacity) : base(capacity) { }

        public void Write(BinaryWriter writer, int depth = 0)
        {
            if (Count >= 0x1F)
            {
                writer.Write((byte)((int)Type | 0x1F));
                writer.Write7BitInt(Count);
            }
            else
            {
                writer.Write((byte)((int)Type | Count));
            }

            if (depth < Micheline.MaxRecursionDepth)
            {
                foreach (var item in this)
                    item.Write(writer, depth + 1);
            }
            else
            {
                foreach (var item in this)
                    MichelineBinaryConverter.WriteFlat(writer, item);
            }
        }
    }
}
