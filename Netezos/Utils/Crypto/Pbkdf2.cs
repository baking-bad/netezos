using System.Security.Cryptography;

namespace Netezos.Utils
{
    public class Pbkdf2 : Stream
    {
        #region PBKDF2
        readonly byte[] Digest;
        readonly byte[] DigestT1;
        readonly byte[] SaltBuffer;
        readonly KeyedHashAlgorithm HmacAlgorithm;
        readonly int Iterations;

        public Pbkdf2(KeyedHashAlgorithm hmacAlgorithm, byte[] salt, int iterations)
        {
            var hmacLength = hmacAlgorithm.HashSize / 8;
            Digest = new byte[hmacLength];
            DigestT1 = new byte[hmacLength];
            SaltBuffer = new byte[salt.Length + 4];
            HmacAlgorithm = hmacAlgorithm;
            Iterations = iterations;
            Array.Copy(salt, SaltBuffer, salt.Length);
        }

        public byte[] Read(int count)
        {
            var buffer = new byte[count];
            var num = Read(buffer, 0, count);
            if (num < count)
                throw new ArgumentException("Can only return {0} bytes.");
            return buffer;
        }

        public static byte[] ComputeDerivedKey(KeyedHashAlgorithm hmacAlgorithm, byte[] salt, int iterations, int derivedKeyLength)
        {
            using var kdf = new Pbkdf2(hmacAlgorithm, salt, iterations);
            return kdf.Read(derivedKeyLength);
        }

        static void BEBytesFromUInt32(uint value, byte[] bytes, int offset)
        {
            bytes[offset + 0] = (byte)(value >> 24);
            bytes[offset + 1] = (byte)(value >> 16);
            bytes[offset + 2] = (byte)(value >> 8);
            bytes[offset + 3] = (byte)(value);
        }

        void ComputeBlock(uint pos)
        {
            BEBytesFromUInt32(pos, SaltBuffer, SaltBuffer.Length - 4);
            ComputeHmac(SaltBuffer, DigestT1);
            Array.Copy(DigestT1, Digest, DigestT1.Length);

            for (int i = 1; i < Iterations; i++)
            {
                ComputeHmac(DigestT1, DigestT1);
                for (int j = 0; j < Digest.Length; j++)
                {
                    Digest[j] ^= DigestT1[j];
                }
            }
        }

        void ComputeHmac(byte[] input, byte[] output)
        {
            HmacAlgorithm.Initialize();
            HmacAlgorithm.TransformBlock(input, 0, input.Length, input, 0);
            HmacAlgorithm.TransformFinalBlock([], 0, 0);
            Array.Copy(HmacAlgorithm.Hash!, output, output.Length);
        }
        #endregion

        #region Stream
        long BlockStart;
        long BlockEnd;
        long Pos;

        public override void Flush() { }

        public override void Close()
        {
            HmacAlgorithm.Clear();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytes = 0;
            while (count > 0)
            {
                if (Position < BlockStart || Position >= BlockEnd)
                {
                    if (Position >= Length)
                    {
                        break;
                    }
                    long pos = Position / Digest.Length;
                    ComputeBlock((uint)(pos + 1));
                    BlockStart = pos * Digest.Length;
                    BlockEnd = BlockStart + Digest.Length;
                }
                var bytesSoFar = (int)(Position - BlockStart);
                var bytesThisTime = Math.Min(Digest.Length - bytesSoFar, count);
                Array.Copy(Digest, bytesSoFar, buffer, bytes, bytesThisTime);
                count -= bytesThisTime;
                bytes += bytesThisTime;
                Position += bytesThisTime;
            }
            return bytes;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            Position = origin switch
            {
                SeekOrigin.Begin => offset,
                SeekOrigin.Current => Position + offset,
                SeekOrigin.End => Length + offset,
                _ => throw new ArgumentException("Invalid seek origin")
            };
            return Position;
        }

        public override void SetLength(long value) => throw new NotImplementedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotImplementedException();

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => false;

        public override long Length => Digest.Length * uint.MaxValue;

        public override long Position { get => Pos; set => Pos = value; }
        #endregion
    }
}
