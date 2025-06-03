using Netezos.Encoding;

namespace Netezos.Keys
{
    public class Signature(byte[] bytes, byte[] prefix)
    {
        public byte[] ToBytes() => bytes;

        public string ToBase58() => Base58.Convert(bytes, prefix);

        public string ToHex() => Hex.Convert(bytes);

        public override string ToString() => ToBase58();

        public static implicit operator byte[] (Signature s) => s.ToBytes();
        
        public static implicit operator string (Signature s) => s.ToBase58();
    }
}