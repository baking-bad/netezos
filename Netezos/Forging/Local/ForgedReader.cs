using Netezos.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Netezos.Forging
{
    public class ForgedReader : IDisposable
    {
        private BinaryReader _reader;

        public bool EndOfStream
        {
            get
            {
                return _reader.BaseStream.Position == _reader.BaseStream.Length;
            }
        }

        public long StreamPosition
        {
            get
            {
                return _reader.BaseStream.Position;
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
            _reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen);
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }

        public IMicheline ReadMicheline()
        {
            var tag = ReadByte();

            switch (tag)
            {
                case 0: return ReadMichelineInt();
                case 1: return ReadMichelineString();
                case 2: return ReadMichelineArray();
                case 3: return ReadMichelinePrimitive(0, false);
                case 4: return ReadMichelinePrimitive(0, true);
                case 5: return ReadMichelinePrimitive(1, false);
                case 6: return ReadMichelinePrimitive(1, true);
                case 7: return ReadMichelinePrimitive(2, false);
                case 8: return ReadMichelinePrimitive(2, true);
                case 9: return ReadMichelinePrimitive(3, true);
                case 10: return ReadMichelineBytes();
                default: throw new InvalidOperationException($"Unknown tag {tag} at position {_reader.BaseStream.Position}");
            }
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
            return new MichelineBytes(ReadArrayData());
        }

        public MichelineArray ReadMichelineArray()
        {
            var arrayData = ReadArrayData();

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
                            var annotVal = a.Substring(1);

                            IAnnotation annotation;

                            switch (a[0])
                            {
                                case FieldAnnotation.Prefix:
                                    annotation = new FieldAnnotation(annotVal);
                                    break;

                                case TypeAnnotation.Prefix:
                                    annotation = new TypeAnnotation(annotVal);
                                    break;

                                case VariableAnnotation.Prefix:
                                    annotation = new VariableAnnotation(annotVal);
                                    break;

                                default:
                                    throw new InvalidOperationException($"Unknown annotation type: {a[0]}");
                            }

                            return annotation;
                        })
                        .ToList();
                }
            }

            return prim;
        }

        public string ReadPublicKey()
        {
            var id = ReadByte();

            byte[] prefix;
            switch (id)
            {
                case 0: prefix = Prefix.edpk; break;
                case 1: prefix = Prefix.sppk; break;
                case 2: prefix = Prefix.p2pk; break;
                default: throw new ArgumentException($"Invalid public key prefix {id}");
            };

            return ReadBase58(32, prefix);
        }

        public string ReadAddress()
        {
            var type = ReadByte();

            switch (type)
            {
                case 0: return ReadTzAddress();
                case 1: return ReadKtAddress();
                default: throw new ArgumentException($"Invalid address prefix {type}");
            }
        }

        public string ReadTzAddress()
        {
            var tzType = ReadByte();

            byte[] prefix;
            switch (tzType)
            {
                case 0: prefix = Prefix.tz1; break;
                case 1: prefix = Prefix.tz2; break;
                case 2: prefix = Prefix.tz3; break;
                default:
                    throw new ArgumentException($"Invalid source prefix {tzType}");
            }

            return ReadBase58(20, prefix);
        }

        public string ReadKtAddress()
        {
            var address = ReadBase58(Lengths.KT1.Decoded, Prefix.KT1);
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
            var epType = ReadByte();

            switch (epType)
            {
                case 0: return "default";
                case 1: return "root";
                case 2: return "do";
                case 3: return "set_delegate";
                case 4: return "remove_delegate";
                case 255: return ReadString();
                default: throw new ArgumentException($"Invalid entrypoint prefix {epType}");
            }
        }

        public bool ReadBool()
        {
            return ReadByte() == 255;
        }

        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        public byte[] ReadBytes(int count)
        {
            return _reader.ReadBytes(count);
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

                using (var reader = new ForgedReader(arrData))
                {
                    var result = readData(reader);

                    if (!reader.EndOfStream)
                    {
                        throw new InvalidOperationException("Expected end of stream but not reached");
                    }

                    return result;
                }
            }
            return default(T);
        }

        public IEnumerable<T> ReadEnumerable<T>(Func<ForgedReader, T> readData)
        {
            if (!EndOfStream)
            {
                var arrLen = ReadInt32();
                var arrData = ReadBytes(arrLen);

                using (var reader = new ForgedReader(arrData))
                {
                    while (!reader.EndOfStream)
                    {
                        yield return readData(reader);
                    }
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

        private byte[] ReadArrayData()
        {
            var arrLen = ReadInt32();
            return ReadBytes(arrLen);
        }
    }
}
