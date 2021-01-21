using System.IO;
using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    [InterfaceJsonConverter(typeof(MichelineConverter))]
    public interface IMicheline
    {
        MichelineType Type { get; }

        void Write(BinaryWriter writer);
    }

    public enum MichelineType : byte
    {
        Int     = 0b_0000_0000,
        Bytes   = 0b_0010_0000,
        String  = 0b_0100_0000,
        Array   = 0b_0110_0000,
        Prim    = 0b_1000_0000
    }
}
