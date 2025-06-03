using Netezos.Utils;

namespace Netezos.Keys
{
    class Eip2333 : HDStandard
    {
        public override (byte[], byte[]) GenerateMasterKey(Curve curve, byte[] seed)
        {
            if (curve is not Bls12381)
                throw new InvalidOperationException("Eip2333 is only for BLS1231");

            byte[] res = new byte[32];
            Blst.blst_derive_master_eip2333(res, seed, (nuint)seed.Length);

            return (res, []);
        }

        public override (byte[], byte[]) GetChildPrivateKey(Curve curve, byte[] privateKey, byte[] chainCode, uint index)
        {
            if (curve is not Bls12381)
                throw new InvalidOperationException("Eip2333 is only for BLS1231");

            byte[] res = new byte[32];
            Blst.blst_derive_child_eip2333(res, privateKey, index);

            return (res, []);
        }

        public override (byte[], byte[]) GetChildPublicKey(Curve curve, byte[] pubKey, byte[] chainCode, uint index)
        {
            throw new NotSupportedException("Eip2333 doesn't support public keys derivation");
        }
    }
}