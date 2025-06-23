using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Keys
{
    class Bls12381 : Curve
    {
        static byte[]? _DstPop;
        static byte[] DstPop => _DstPop ??= Utf8.Parse("BLS_POP_BLS12381G2_XMD:SHA-256_SSWU_RO_POP_");

        static byte[]? _DstSig;
        static byte[] DstSig => _DstSig ??= Utf8.Parse("BLS_SIG_BLS12381G2_XMD:SHA-256_SSWU_RO_POP_");
        
        //static byte[] Dst => _Dst ??= Utf8.Parse("BLS_SIG_BLS12381G2_XMD:SHA-256_SSWU_RO_AUG_"); // Aug scheme

        public override ECKind Kind => ECKind.Bls12381;

        public override byte[] AddressPrefix => Prefix.tz4;
        public override byte[] PublicKeyPrefix => Prefix.BLpk;
        public override byte[] PrivateKeyPrefix => Prefix.BLsk;
        public override byte[] SignaturePrefix => Prefix.BLsig;
        public override byte[] Slip10Seed => throw new InvalidOperationException("BLS12-381 doesn't support SLIP-10");

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

        public override byte[] GetPublicKey(byte[] prvKey)
        {
            var pk = new long[Blst.blst_p1_sizeof() / sizeof(long)];
            Blst.blst_sk_to_pk_in_g1(pk, prvKey);

            var res = new byte[48];
            Blst.blst_p1_compress(res, pk);

            return res;
        }

        public override Signature GetProofOfPossession(byte[] prvKey)
        {
            return Sign(GetPublicKey(prvKey), prvKey, DstPop);
        }

        public override Signature Sign(byte[] msg, byte[] prvKey)
        {
            var message = msg;
            //var message = GetPublicKey(prvKey).Concat(msg); // Aug scheme

            return Sign(message, prvKey, DstSig);
        }

        public override bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            BlstError res;

            #region init pairing
            var pairingSize = (int)(Blst.blst_pairing_sizeof() / sizeof(long));
            var paddedDstSize = (DstSig.Length + sizeof(long) - 1) / sizeof(long);
            var paddedDst = new byte[paddedDstSize * sizeof(long)];
            Buffer.BlockCopy(DstSig, 0, paddedDst, 0, DstSig.Length);

            var ctx = new long[pairingSize + paddedDstSize];

            for (int i = 0; i < paddedDstSize; i++)
                ctx[pairingSize + i] = BitConverter.ToInt64(paddedDst, i * sizeof(long));

            Blst.blst_pairing_init(ctx, true, ref ctx[pairingSize], (nuint)DstSig.Length);
            #endregion

            var _msg = msg;
            //var _msg = pubKey.Concat(msg); // Aug scheme

            var _pk = new long[Blst.blst_p1_affine_sizeof() / sizeof(long)];
            res = Blst.blst_p1_uncompress(_pk, pubKey);
            if (res != BlstError.SUCCESS)
                return false;

            var _sig = new long[Blst.blst_p2_affine_sizeof() / sizeof(long)];
            res = Blst.blst_p2_uncompress(_sig, sig);
            if (res != BlstError.SUCCESS)
                return false;

            res = Blst.blst_pairing_chk_n_aggr_pk_in_g1(ctx, _pk, true, _sig, true, _msg, (nuint)_msg.Length, [], 0);
            if (res != BlstError.SUCCESS)
                return false;

            Blst.blst_pairing_commit(ctx);

            return Blst.blst_pairing_finalverify(ctx, []);
        }

        Signature Sign(byte[] msg, byte[] prvKey, byte[] dst)
        {
            var hash = new long[Blst.blst_p2_sizeof() / sizeof(long)];
            Blst.blst_hash_to_g2(hash, msg, (nuint)msg.Length, dst, (nuint)dst.Length, [], 0);

            var sig = new long[Blst.blst_p2_affine_sizeof() / sizeof(long)];
            Blst.blst_sign_pk_in_g1(sig, hash, prvKey);

            var res = new byte[96];
            Blst.blst_p2_affine_compress(res, sig);

            return new Signature(res, SignaturePrefix);
        }
    }
}
