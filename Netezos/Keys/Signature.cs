using Netezos.Encoding;

namespace Netezos.Keys
{
    public class Signature
    {
        readonly byte[] Bytes;
        readonly byte[] Prefix;

        public Signature(byte[] bytes, byte[] prefix)
        {
            Bytes = bytes;
            Prefix = prefix;
        }

        public byte[] ToBytes() => Bytes;

        public string ToBase58() => Base58.Convert(Bytes, Prefix);

        public string ToHex() => Hex.Convert(Bytes);

        public override string ToString() => ToBase58();

        public static implicit operator byte[] (Signature s) => s.ToBytes();
        
        public static implicit operator string (Signature s) => s.ToBase58();
    }
}