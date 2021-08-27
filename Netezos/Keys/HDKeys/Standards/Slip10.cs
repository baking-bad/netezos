using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using Netezos.Keys.HDKeys;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Math;

namespace Netezos.Keys
{
    class Slip10 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Slip10;

        public override byte[] GenerateMasterKey(Curve curve, byte[] seed)
        {
            using var hmacSha512 = new HMACSHA512(curve.SeedKey);
            while (true)
            {
                var l = hmacSha512.ComputeHash(seed);
                if (curve.Kind == ECKind.Ed25519)
                {
                    return l;
                }
                
                var N = curve.Kind switch
                {
                    ECKind.Secp256k1 => SecNamedCurves.GetByName("secp256k1").N,
                    ECKind.NistP256 => SecNamedCurves.GetByName("secp256r1").N,
                    _ => throw new InvalidEnumArgumentException()
                };
                
                var ll = l.GetBytes(0, 32);
                var parse256LL = new BigInteger(1, ll);

                if (parse256LL.CompareTo(N) < 0 && !Equals(parse256LL, BigInteger.Zero)) return l;
                
                seed = l;
            }
        }

        public override byte[] GetChildPrivateKey(Curve curve, byte[] extKey, uint index)
        {
            var ccChild = new byte[4];

            var cc = extKey.GetBytes(32, 32);
            byte[]? l = null;

            if ((index >> 31) == 0)
            {
                var pubKey = curve.GetPublicKey(extKey.GetBytes(0, 32));
                l = BIP32Hash(cc, index, pubKey[0], pubKey.GetBytes(1, pubKey.Length - 1));
            }
            else
            {
                l = BIP32Hash(cc, index, 0, extKey.GetBytes(0, 32));
            }

            if (curve.Kind == ECKind.Ed25519)
            {
                return l;
            }


            while (true)
            {
                var ll = l.GetBytes(0, 32);
                var lr = l.GetBytes(32, 32);

                ccChild = lr;

                var parse256LL = new BigInteger(1, ll);

                //TODO data here is vch NBitcoin
                var kPar = new BigInteger(1, extKey.GetBytes(0, 32));

                var keyBytes = new byte[4];

                var N = curve.Kind switch
                {
                    ECKind.Secp256k1 => SecNamedCurves.GetByName("secp256k1").N,
                    ECKind.NistP256 => SecNamedCurves.GetByName("secp256r1").N,
                    _ => throw new InvalidEnumArgumentException()
                };

                var key = parse256LL.Add(kPar).Mod(N);

                if (parse256LL.CompareTo(N) >= 0 || key == BigInteger.Zero)
                {
                    l = BIP32Hash(cc, index, 1, lr);
                    continue;
                }

                keyBytes = key.ToByteArrayUnsigned();
                if (keyBytes.Length < 32)
                    keyBytes = new byte[32 - keyBytes.Length].Concat(keyBytes).ToArray();

                return keyBytes.Concat(ccChild);
            }
        }

        public override (byte[], byte[]) GetChildPublicKey(Curve curve, byte[] pubKey, byte[] chainCode, uint index)
        {
            var l = new byte[32];
            var r = new byte[32];
            var lr = BIP32Hash(chainCode, index, pubKey[0], pubKey.Skip(1).ToArray());
            Array.Copy(lr, l, 32);
            Array.Copy(lr, 32, r, 0, 32);

            return (curve.GetPublicKey(l), r);
            
            if (curve.Kind == ECKind.Ed25519)
            {
                return (l, r);
            }
            
            var N = curve.Kind switch
            {
                ECKind.Secp256k1 => SecNamedCurves.GetByName("secp256k1").N,
                ECKind.NistP256 => SecNamedCurves.GetByName("secp256r1").N,
                _ => throw new InvalidEnumArgumentException()
            };

            return (curve.GetPublicKey(l), r);
            
            /*
            BigInteger parse256LL = new BigInteger(1, l);

            if (parse256LL.CompareTo(N) >= 0)
                throw new InvalidOperationException("You won a prize ! this should happen very rarely. Take a screenshot, and roll the dice again.");

            var q = ECKey.CURVE.G.Multiply(parse256LL).Add(ECKey.GetPublicKeyParameters().Q);
            if (q.IsInfinity)
                throw new InvalidOperationException("You won the big prize ! this would happen only 1 in 2^127. Take a screenshot, and roll the dice again.");

            q = q.Normalize();
            var p = new NBitcoin.BouncyCastle.Math.EC.FpPoint(ECKey.CURVE.Curve, q.XCoord, q.YCoord, true);
            return new PubKey(p.GetEncoded());*/
        }

        public override byte[] GetChildPublicKey(Curve curve, byte[] privateKey)
        {
            var publicKey = curve.GetPublicKey(privateKey);

            var pubBuffer = new BigEndianBuffer();

            pubBuffer.Write(publicKey);

            return curve.GetPublicKey(privateKey);
        }

        public static byte[] BIP32Hash(byte[] chainCode, uint nChild, byte header, byte[] data)
        {
            byte[] num = new byte[4];
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