using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Netezos.Keys.HDKeys;

namespace Netezos.Keys
{
    class Slip10 : HDStandard
    {
        public override HDStandardKind Kind => HDStandardKind.Slip10;

        public override byte[] GenerateMasterKey(Curve curve, byte[] seed)
        {
            using (var hmacSha512 = new HMACSHA512(curve.SeedKey))
            {
                return hmacSha512.ComputeHash(seed);
            }
        }

        public override byte[] GetChildPrivateKey(Curve curve, byte[] extKey, uint index)
        {
            var buffer = new BigEndianBuffer();

            buffer.Write(new byte[] { 0 });
            buffer.Write(extKey.GetBytes(0, 32));
            buffer.WriteUInt(index);

            using (var hmacSha512 = new HMACSHA512(extKey.GetBytes(32, 32)))
            {
                var i = hmacSha512.ComputeHash(buffer.ToArray());

                var il = i.GetBytes(0, 32);
                var ir = i.GetBytes(32,32);

                return i;
            }
        }

        public override byte[] GetChildPublicKey(Curve curve, byte[] extKey, uint index, bool withZeroByte = true)
        {
            var buffer = new BigEndianBuffer();

            buffer.Write(new byte[] { 0 });
            buffer.Write(extKey.GetBytes(0, 32));
            buffer.WriteUInt(index);

            using (var hmacSha512 = new HMACSHA512(extKey.GetBytes(32, 32)))
            {
                var i = hmacSha512.ComputeHash(buffer.ToArray());

                var il = i.GetBytes(0, 32);
                var ir = i.GetBytes(32,32);

                var publicKey = curve.GetPublicKey(i);
                
                var zero = new byte[] { 0 };

                var pubBuffer = new BigEndianBuffer();
                if (withZeroByte)
                    pubBuffer.Write(zero);

                pubBuffer.Write(publicKey);
                
                return pubBuffer.ToArray();
            }
        }
    }
}
