using System;
using System.Security.Cryptography;

namespace Netezos.Keys
{
    class Bip32 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Bip32;

        public override byte[] GenerateMasterKey(Curve curve, byte[] seed)
        {
            using (HMACSHA512 hmacSha512 = new HMACSHA512(curve.SeedKey))
            {
                return hmacSha512.ComputeHash(seed);
            }
        }

        public override byte[] GetChildPrivateKey(Curve curve, byte[] extKey, uint index)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetChildPublicKey(Curve curve, byte[] extKey, uint index)
        {
            throw new NotImplementedException();
        }
    }
}