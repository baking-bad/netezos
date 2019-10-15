using Netezos.Keys.Crypto;

namespace Netezos.Keys
{
    public class Signature
    {
        readonly byte[] Sign;
        readonly ECKind Curve;

        public Signature(byte[] bytes, ECKind curve)
        {
            Sign = bytes;
            Curve = curve;
        }

        public override string ToString()
        {
            return Base58.Convert(Sign, Crypto.Curve.GetCurve(Curve).SignaturePrefix);
        }
        
        public static implicit operator byte[](Signature s)
        {
            return s.Sign;
        }
        
        public static implicit operator string(Signature s)
        {
            return s.ToString();
        }
    }
}