using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Netezos.Keys
{
    class Slip10 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Slip10;

        public override (byte[], byte[]) GenerateMasterKey(Curve curve, byte[] seed)
        {
            using var hmacSha512 = new HMACSHA512(curve.SeedKey);
            while (true)
            {
                var l = hmacSha512.ComputeHash(seed);
                if (curve.Kind == ECKind.Ed25519)
                {
                    return (l.GetBytes(0, 32), l.GetBytes(32,32));
                }
                
                var N = curve.Kind switch
                {
                    ECKind.Secp256k1 => SecNamedCurves.GetByName("secp256k1").N,
                    ECKind.NistP256 => SecNamedCurves.GetByName("secp256r1").N,
                    _ => throw new InvalidEnumArgumentException()
                };
                
                var ll = l.GetBytes(0, 32);
                var parse256LL = new BigInteger(1, ll);

                if (parse256LL.CompareTo(N) < 0 && !Equals(parse256LL, BigInteger.Zero))
                    return (l.GetBytes(0, 32), l.GetBytes(32,32));
                
                seed = l;
            }
        }

        public override (byte[], byte[]) GetChildPrivateKey(Curve curve, byte[] privateKey, byte[] chainCode, uint index)
        {

            byte[] l;

            if ((index >> 31) == 0)
            {
                var pubKey = curve.GetPublicKey(privateKey);
                l = Bip32Hash(chainCode, index, pubKey[0], pubKey.GetBytes(1, pubKey.Length - 1));
            }
            else
            {
                l = Bip32Hash(chainCode, index, 0, privateKey);
            }

            if (curve.Kind == ECKind.Ed25519)
            {
                return (l.GetBytes(0,32), l.GetBytes(32,32));
            }

            while (true)
            {
                var lr = l.GetBytes(32, 32);

                var parse256LL = new BigInteger(1, l.GetBytes(0, 32));

                //data here is vch NBitcoin
                var kPar = new BigInteger(1, privateKey);

                var N = curve.Kind switch
                {
                    ECKind.Secp256k1 => SecNamedCurves.GetByName("secp256k1").N,
                    ECKind.NistP256 => SecNamedCurves.GetByName("secp256r1").N,
                    _ => throw new InvalidEnumArgumentException()
                };

                var key = parse256LL.Add(kPar).Mod(N);

                if (parse256LL.CompareTo(N) >= 0 || key == BigInteger.Zero)
                {
                    l = Bip32Hash(chainCode, index, 1, lr);
                    continue;
                }

                var keyBytes = key.ToByteArrayUnsigned();
                if (keyBytes.Length < 32)
                    keyBytes = new byte[32 - keyBytes.Length].Concat(keyBytes).ToArray();

                return (keyBytes, lr);
            }
        }

        public override (byte[], byte[]) GetChildPublicKey(Curve curve, byte[] pubKey, byte[] chainCode, uint index)
        {
            if (curve.Kind == ECKind.Ed25519)
                throw new NotSupportedException("Ed25519 public key derivation not supported by slip-10");
            if (pubKey.Length != 33)
                throw new NotSupportedException("Invalid public key size");
            if ((index >> 31) != 0)
                throw new InvalidOperationException("A public key can't derivate a hardened child");

            var l = new byte[32];
            var r = new byte[32];
            var lr = Bip32Hash(chainCode, index, pubKey[0], pubKey.Skip(1).ToArray());
            Array.Copy(lr, l, 32);
            Array.Copy(lr, 32, r, 0, 32);
            
            var c = curve.Kind switch
            {
                ECKind.Secp256k1 => SecNamedCurves.GetByName("secp256k1"),
                ECKind.NistP256 => SecNamedCurves.GetByName("secp256r1"),
                _ => throw new InvalidEnumArgumentException()
            };
            
            var domainParameters = new ECDomainParameters(c.Curve, c.G, c.N, c.H, c.GetSeed());
            var keyParameters = new ECPublicKeyParameters("EC", c.Curve.DecodePoint(pubKey),
                domainParameters);

            while (true)
            {
                Array.Copy(lr, l, 32);
                Array.Copy(lr, 32, r, 0, 32);
                
                var parse256LL = new BigInteger(1, l);
            
                var q = keyParameters.Parameters.G.Multiply(parse256LL).Add(keyParameters.Q);

                if (parse256LL.CompareTo(c.N) >= 0 || q.IsInfinity)
                {
                    lr = Bip32Hash(chainCode, index, 1, r);
                    continue;
                }
                
                q = q.Normalize();
                var b = domainParameters.Curve.CreatePoint(q.XCoord.ToBigInteger(), q.YCoord.ToBigInteger()).GetEncoded(true);
                return (b, r);
            }
        }

        static byte[] Bip32Hash(byte[] chainCode, uint nChild, byte header, byte[] data)
        {
            var num = new byte[4];
            num[0] = (byte)((nChild >> 24) & 0xFF);
            num[1] = (byte)((nChild >> 16) & 0xFF);
            num[2] = (byte)((nChild >> 8) & 0xFF);
            num[3] = (byte)((nChild >> 0) & 0xFF);

            using var hmacSha512 = new HMACSHA512(chainCode);
            var i = hmacSha512.ComputeHash(new byte[] { header }
                .Concat(data)
                .Concat(num).ToArray());

            return i;
        }
    }
}