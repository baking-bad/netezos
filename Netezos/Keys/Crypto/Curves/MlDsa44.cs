namespace Netezos.Keys
{
    class MlDsa44 : Curve
    {
        #region static
        public const int PublicKeySize = 1312;
        public const int SigningKeySize = 2560;
        public const int PrivateKeySize = SigningKeySize + PublicKeySize;
        public const int SignatureSize = 2420;
        #endregion

        public override ECKind Kind => ECKind.MlDsa44;

        public override byte[] AddressPrefix => Prefixes.tz5;
        public override byte[] PublicKeyPrefix => Prefixes.mdpk;
        public override byte[] PrivateKeyPrefix => Prefixes.mdsk;
        public override byte[] SignaturePrefix => Prefixes.mdsig;
        public override byte[] Slip10Seed => throw new InvalidOperationException("ML-DSA-44 doesn't support SLIP-10");

        public override byte[] ExtractPrivateKey(byte[] bytes)
        {
            if (bytes.Length != PrivateKeySize)
                throw new ArgumentException($"Invalid private key length. Expected {PrivateKeySize} bytes.");

            return bytes.GetBytes(0, PrivateKeySize);
        }

        public override byte[] GeneratePrivateKey()
        {
            throw new NotImplementedException("ML-DSA-44 key generation is not implemented");
        }

        public override byte[] GetPublicKey(byte[] prvKey)
        {
            // Octez stores the verification key right after the signing key
            return prvKey.GetBytes(SigningKeySize, PublicKeySize);
        }

        public override Signature GetProofOfPossession(byte[] prvKey)
        {
            throw new NotImplementedException("ML-DSA-44 proof of possession is not implemented");
        }

        public override Signature Sign(byte[] msg, byte[] prvKey)
        {
            throw new NotImplementedException("ML-DSA-44 signing is not implemented");
        }

        public override bool Verify(byte[] msg, byte[] sig, byte[] pubKey)
        {
            throw new NotImplementedException("ML-DSA-44 signature verification is not implemented");
        }
    }
}
