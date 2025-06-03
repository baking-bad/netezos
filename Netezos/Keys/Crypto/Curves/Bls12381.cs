using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Keys
{
    class Bls12381 : Curve
    {
        static byte[]? _Dst;
        static byte[] Dst => _Dst ??= Utf8.Parse("BLS_SIG_BLS12381G2_XMD:SHA-256_SSWU_RO_AUG_");

        public override ECKind Kind => ECKind.Bls12381;

        public override byte[] AddressPrefix => Prefix.tz4;
        public override byte[] PublicKeyPrefix => Prefix.BLpk;
        public override byte[] PrivateKeyPrefix => Prefix.BLsk;
        public override byte[] SignaturePrefix => Prefix.BLsig;
        public override byte[] Slip10Seed => throw new NotImplementedException("BLS12-381 curve is not implemented yet");

        public override byte[] ExtractPrivateKey(byte[] bytes)
        {
            if (bytes.Length != 32)
                throw new ArgumentException("Invalid private key length. Expected 32 bytes.");

            return bytes.GetBytes(0, 32);
        }

        public override byte[] GeneratePrivateKey()
        {
            byte[] ikm = new byte[32];
            RNG.WriteBytes(ikm);

            byte[] res = new byte[32];
            Blst.blst_keygen(res, ikm, 32, [], 0);

            return res;
        }

        public override byte[] GetPublicKey(byte[] privateKey)
        {
            var pk = new long[Blst.blst_p1_sizeof() / sizeof(long)];
            Blst.blst_sk_to_pk_in_g1(pk, privateKey);

            var res = new byte[48];
            Blst.blst_p1_compress(res, pk);

            return res;
        }

        public override Signature Sign(byte[] msg, byte[] prvKey)
        {
            var message = GetPublicKey(prvKey).Concat(msg); // TODO: re-check after migrating from AUG to POP

            var hash = new long[Blst.blst_p2_sizeof() / sizeof(long)];
            Blst.blst_hash_to_g2(hash, message, (nuint)message.Length, Dst, (nuint)Dst.Length, [], 0);

            var sig = new long[Blst.blst_p2_affine_sizeof() / sizeof(long)];
            Blst.blst_sign_pk_in_g1(sig, hash, prvKey);

            var res = new byte[96];
            Blst.blst_p2_affine_compress(res, sig);

            return new Signature(res, SignaturePrefix);
        }

        public override bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            Blst.ERROR res;

            #region init pairing
            var pairingSize = (int)(Blst.blst_pairing_sizeof() / sizeof(long));
            var paddedDstSize = (Dst.Length + sizeof(long) - 1) / sizeof(long);
            var paddedDst = new byte[paddedDstSize * sizeof(long)];
            Buffer.BlockCopy(Dst, 0, paddedDst, 0, Dst.Length);

            var ctx = new long[pairingSize + paddedDstSize];

            for (int i = 0; i < paddedDstSize; i++)
                ctx[pairingSize + i] = BitConverter.ToInt64(paddedDst, i * sizeof(long));

            Blst.blst_pairing_init(ctx, true, ref ctx[pairingSize], (nuint)Dst.Length);
            #endregion

            var _msg = pubKey.Concat(msg);

            var _pk = new long[Blst.blst_p1_affine_sizeof() / sizeof(long)];
            res = Blst.blst_p1_uncompress(_pk, pubKey);
            if (res != Blst.ERROR.SUCCESS)
                return false;

            var _sig = new long[Blst.blst_p2_affine_sizeof() / sizeof(long)];
            res = Blst.blst_p2_uncompress(_sig, sig);
            if (res != Blst.ERROR.SUCCESS)
                return false;

            res = Blst.blst_pairing_chk_n_aggr_pk_in_g1(ctx, _pk, true, _sig, true, _msg, (nuint)_msg.Length, [], 0);
            if (res != Blst.ERROR.SUCCESS)
                return false;

            Blst.blst_pairing_commit(ctx);

            return Blst.blst_pairing_finalverify(ctx, []);
        }
    }
}
