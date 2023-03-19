using System.Numerics;
using Netezos.Encoding;

namespace Netezos.Forging
{
    public class ForgedReader : IDisposable
    {
        readonly BinaryReader Reader;

        public bool EndOfStream
        {
            get
            {
                return Reader.BaseStream.Position == Reader.BaseStream.Length;
            }
        }

        public long StreamPosition
        {
            get
            {
                return Reader.BaseStream.Position;
            }
        }

        public ForgedReader(byte[] data)
            : this(new MemoryStream(data), false)
        { }

        public ForgedReader(Stream stream)
            : this(stream, true)
        { }

        private ForgedReader(Stream stream, bool leaveOpen)
        {
            Reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen);
        }

        public void Dispose()
        {
            Reader?.Dispose();
        }

        public IMicheline ReadMicheline()
        {
            return ReadByte() switch
            {
                0 => ReadMichelineInt(),
                1 => ReadMichelineString(),
                2 => ReadMichelineArray(),
                3 => ReadMichelinePrimitive(0, false),
                4 => ReadMichelinePrimitive(0, true),
                5 => ReadMichelinePrimitive(1, false),
                6 => ReadMichelinePrimitive(1, true),
                7 => ReadMichelinePrimitive(2, false),
                8 => ReadMichelinePrimitive(2, true),
                9 => ReadMichelinePrimitive(3, true),
                10 => ReadMichelineBytes(),
                var tag => throw new InvalidOperationException($"Unknown tag {tag} at position {Reader.BaseStream.Position}")
            };
        }

        public MichelineInt ReadMichelineInt()
        {
            return new MichelineInt(ReadSBigInt());
        }

        public MichelineString ReadMichelineString()
        {
            return new MichelineString(ReadString());
        }

        public MichelineBytes ReadMichelineBytes()
        {
            return new MichelineBytes(ReadArray());
        }

        public MichelineArray ReadMichelineArray()
        {
            var arrayData = ReadArray();

            var res = new MichelineArray();

            using (var arr = new ForgedReader(arrayData))
            {
                while (!arr.EndOfStream)
                {
                    res.Add(arr.ReadMicheline());
                }
            }

            return res;
        }

        private MichelinePrim ReadMichelinePrimitive(int argsLength, bool annotations = false)
        {
            var primTag = (PrimType)ReadByte();

            var prim = new MichelinePrim
            {
                Prim = primTag
            };

            if (0 < argsLength && argsLength < 3)
            {
                prim.Args = new MichelineArray(argsLength);

                for (var i = 0; i < argsLength; i++)
                {
                    prim.Args.Add(ReadMicheline());
                }
            }
            else if (argsLength == 3)
            {
                prim.Args = ReadMichelineArray();
            }
            else if (argsLength != 0)
            {
                throw new ArgumentException($"Unexpected args length {argsLength}", nameof(argsLength));
            }

            if (annotations)
            {
                var annots = ReadString();

                if (annots.Length > 0)
                {
                    prim.Annots = annots
                        .Split(' ')
                        .Select(a =>
                        {
                            var annot = a.Substring(1);
                            return a[0] switch
                            {
                                FieldAnnotation.Prefix => (IAnnotation)new FieldAnnotation(annot),
                                TypeAnnotation.Prefix => new TypeAnnotation(annot),
                                VariableAnnotation.Prefix => new VariableAnnotation(annot),
                                _ => throw new InvalidOperationException($"Unknown annotation type: {a[0]}")
                            };
                        })
                        .ToList();
                }
            }

            return prim;
        }

        public string ReadPublicKey()
        {
            var (prefix, len) = ReadByte() switch
            {
                0 => (Prefix.edpk, 32),
                1 => (Prefix.sppk, 33),
                2 => (Prefix.p2pk, 33),
                3 => (Prefix.BLpk, 48),
                var type => throw new ArgumentException($"Invalid public key prefix {type}")
            };

            return ReadBase58(len, prefix);
        }

        public string ReadAddress()
        {
            return ReadByte() switch
            {
                0 => ReadTzAddress(),
                1 => ReadKtAddress(),
                2 => ReadTxrAddress(),
                3 => ReadSrAddress(),
                var type => throw new ArgumentException($"Invalid address prefix {type}")
            };
        }

        public string ReadTzAddress()
        {
            var prefix = ReadByte() switch
            {
                0 => Prefix.tz1,
                1 => Prefix.tz2,
                2 => Prefix.tz3,
                3 => Prefix.tz4,
                var type => throw new ArgumentException($"Invalid source prefix {type}")
            };
            return ReadBase58(20, prefix);
        }

        public string ReadKtAddress()
        {
            var address = ReadBase58(Lengths.KT1.Decoded, Prefix.KT1);
            ReadByte(); // Consume padded 0
            return address;
        }

        public string ReadTxrAddress()
        {
            var address = ReadBase58(Lengths.txr1.Decoded, Prefix.txr1);
            ReadByte(); // Consume padded 0
            return address;
        }

        public string ReadSrAddress()
        {
            var address = ReadBase58(Lengths.sr1.Decoded, Prefix.sr1);
            ReadByte(); // Consume padded 0
            return address;
        }

        /// <summary>
        /// Read a Micheline natural.
        /// </summary>
        /// <returns>A Micheline natural.</returns>
        public BigInteger ReadUBigInt()
        {
            var value = BigInteger.Zero;

            var bytes = new List<byte>();

            byte b;
            while (((b = ReadByte()) & 0x80) != 0)
            {
                bytes.Add(b);
            }
            bytes.Add(b);

            for (var i = bytes.Count - 1; i >= 0; i--)
            {
                value <<= 7;
                value |= (bytes[i] & 0x7F);
            }

            return value;
        }

        /// <summary>
        /// Read a Micheline integer.
        /// </summary>
        /// <returns>A Micheline integer.</returns>
        public BigInteger ReadSBigInt()
        {
            var value = BigInteger.Zero;

            var bytes = new List<byte>();

            byte b;
            while (((b = ReadByte()) & 0x80) != 0)
            {
                bytes.Add(b);
            }
            bytes.Add(b);

            for (var i = bytes.Count - 1; i > 0; i--)
            {
                value <<= 7;
                value |= (bytes[i] & 0x7F);
            }

            value <<= 6;
            value |= bytes[0] & 0x3F;

            if ((bytes[0] & 0x40) != 0)
            {
                value = -value;
            }

            return value;
        }

        public string ReadEntrypoint()
        {
            return ReadByte() switch
            {
                0 => "default",
                1 => "root",
                2 => "do",
                3 => "set_delegate",
                4 => "remove_delegate",
                5 => "deposit",
                255 => ReadString(),
                var ep => throw new ArgumentException($"Invalid entrypoint prefix {ep}")
            };
        }

        public bool ReadBool()
        {
            return ReadByte() == 255;
        }

        public byte ReadByte()
        {
            return Reader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return Reader.ReadBytes(count);
        }

        public int ReadInt32(int len = 4)
        {
            if (len < 1 || 4 < len)
                throw new ArgumentOutOfRangeException($"{nameof(len)} must be between 1 and 4");

            var bytes = ReadBytes(len);

            var res = 0;
            for (int i = 0, shift = ((len - 1) * 8); i < len; i++, shift -= 8)
            {
                res |= bytes[i] << shift;
            }

            return res;
        }

        public long ReadInt64(int len = 8)
        {
            if (len < 1 || 8 < len)
                throw new ArgumentOutOfRangeException($"{nameof(len)} must be between 1 and 8");

            var bytes = ReadBytes(len);

            var i1 = 0;
            var i2 = 0;
            var idx = 0;

            var highIterations = Math.Max(len - 4, 0);
            var highShift = Math.Max(((highIterations) - 1) * 8, 0);
            var lowIterations = len - highIterations;
            var lowShift = (lowIterations - 1) * 8;

            for (int i = 0, shift = highShift; i < highIterations; i++, shift -= 8, idx++)
                i1 |= bytes[idx] << shift;

            for (int i = 0, shift = lowShift; i < lowIterations; i++, shift -= 8, idx++)
                i2 |= bytes[idx] << shift;

            return (uint)i2 | ((long)i1 << 32);
        }

        public string ReadString(int len = 4)
        {
            var stringLength = ReadInt32(len);
            return Utf8.Convert(ReadBytes(stringLength));
        }

        public string ReadTz1Address()
        {
            return ReadBase58(Lengths.tz1.Decoded, Prefix.tz1);
        }

        public T ReadEnumerableSingle<T>(Func<ForgedReader, T> readData)
        {
            if (!EndOfStream)
            {
                var arrLen = ReadInt32();
                var arrData = ReadBytes(arrLen);

                using var reader = new ForgedReader(arrData);
                var result = readData(reader);

                if (!reader.EndOfStream)
                {
                    throw new InvalidOperationException("Expected end of stream but not reached");
                }

                return result;
            }
            throw new InvalidOperationException("Cannot read from end of stream");
        }

        public IEnumerable<T> ReadEnumerable<T>(Func<ForgedReader, T> readData)
        {
            if (!EndOfStream)
            {
                var arrLen = ReadInt32();
                var arrData = ReadBytes(arrLen);

                using var reader = new ForgedReader(arrData);
                while (!reader.EndOfStream)
                {
                    yield return readData(reader);
                }
            }
            yield break;
        }

        public string ReadHexString()
        {
            var stringLength = ReadInt32();
            return Hex.Convert(ReadBytes(stringLength));
        }

        public string ReadBase58(int length, byte[] prefix)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            else if (prefix.Length == 0)
            {
                throw new ArgumentException("Prefix length must be greater than 0", nameof(prefix));
            }

            var b58bytes = ReadBytes(length);
            return Base58.Convert(b58bytes, prefix);
        }

        public byte[] ReadArray()
        {
            var count = ReadInt32();
            return ReadBytes(count);
        }
    }
}
