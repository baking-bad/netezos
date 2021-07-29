using System;
using System.Security.Cryptography;

namespace Netezos.Keys
{
    class Bip32 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Bip32;

        public override byte[] GenerateMasterKey(Curve curve, byte[] seed)
        {
            using (var hmacSha512 = new HMACSHA512(curve.SeedKey))
            {
                return hmacSha512.ComputeHash(seed);
            }
            
            /*Generate a seed byte sequence S of a chosen length (between 128 and 512 bits; 256 bits is advised) from a (P)RNG.
Calculate I = HMAC-SHA512(Key = "Bitcoin seed", Data = S)
Split I into two 32-byte sequences, IL and IR.
Use parse256(IL) as master secret key, and IR as master chain code.
In case IL is 0 or ≥n, the master key is invalid.*/
            
            // var array = HMACSHA512(curve.SeedKey, seed);
            // var priv = array.GetBytes(0, 32);
            // var pub = array.GetBytes(32, 32);

        }

        public override byte[] GetChildPrivateKey(Curve curve, byte[] extKey, uint index)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetChildPublicKey(Curve curve, byte[] extKey, uint index, bool withZeroByte)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetChildPublicKey(Curve curve, byte[] privateKey, bool withZeroByte = true)
        {
            throw new NotImplementedException();
        }
    }
}
