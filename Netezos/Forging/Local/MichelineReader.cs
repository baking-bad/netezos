using Netezos.Encoding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Netezos.Forging
{
    public class MichelineReader : IDisposable
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

        public MichelineReader(byte[] data)
            : this(new MemoryStream(data), false)
        { }

        public MichelineReader(Stream stream)
            : this(stream, true)
        { }

        private MichelineReader(Stream stream, bool leaveOpen)
        {
            _reader = new BinaryReader(stream, System.Text.Encoding.UTF8, leaveOpen);
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }

        public IMicheline ReadMicheline()
        {
            byte tag = ReadByte();

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
            byte[] arrayData = ReadArrayData();

            MichelineArray res = new MichelineArray();

            using (MichelineReader arr = new MichelineReader(arrayData))
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
            PrimType primTag = (PrimType)ReadByte();

            MichelinePrim prim = new MichelinePrim
            {
                Prim = primTag
            };

            if (0 < argsLength && argsLength < 3)
            {
                prim.Args = new MichelineArray(argsLength);

                for (int i = 0; i < argsLength; i++)
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
                string annots = ReadString();

                if (annots.Length > 0)
                {
                    prim.Annots = annots
                        .Split(' ')
                        .Select(a =>
                        {
                            string annotVal = a.Substring(1);

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
            byte[] prefix;

            byte id = ReadByte();

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
            byte type = ReadByte();

            switch (type)
            {
                case 0: return ReadTzAddress();
                case 1: return ReadKtAddress();
                default: throw new ArgumentException($"Invalid address prefix {type}");
            }
        }

        public string ReadTzAddress()
        {
            byte[] prefix;

            byte tzType = ReadByte();

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
            string address = ReadBase58(Lengths.KT1.Decoded, Prefix.KT1);
            ReadByte(); // Consume padded 0
            return address;
        }

        /// <summary>
        /// Read a Micheline natural.
        /// </summary>
        /// <returns>A Micheline natural.</returns>
        public BigInteger ReadUBigInt()
        {
            BigInteger value = 0;

            List<byte> bytes = new List<byte>();

            byte b;
            while (((b = ReadByte()) & 0x80) != 0)
            {
                bytes.Add(b);
            }
            bytes.Add(b);

            for (int i = bytes.Count - 1; i >= 0; i--)
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
            int value = 0;

            List<byte> bytes = new List<byte>();

            byte b;
            while (((b = ReadByte()) & 0x80) != 0)
            {
                bytes.Add(b);
            }
            bytes.Add(b);

            for (int i = bytes.Count - 1; i > 0; i--)
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
            byte epType = ReadByte();

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

        public byte[] ReadBytesToEnd()
        {
            if (EndOfStream)
            {
                return new byte[0];
            }

            List<byte> bytes = new List<byte>();

            while (!EndOfStream)
            {
                bytes.Add(ReadByte());
            }

            return bytes.ToArray();
        }

        public int ReadInt32(int len = 4)
        {
            if (len < 1 || 4 < len)
                throw new ArgumentOutOfRangeException($"{nameof(len)} must be between 1 and 4");

            byte[] bytes = ReadBytes(len);

            int res = 0;

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

            byte[] bytes = ReadBytes(len);

            int i1 = 0;
            int i2 = 0;
            int idx = 0;

            int highIterations = Math.Max(len - 4, 0);
            int highShift = Math.Max(((highIterations) - 1) * 8, 0);
            int lowIterations = len - highIterations;
            int lowShift = (lowIterations - 1) * 8;

            for (int i = 0, shift = highShift; i < highIterations; i++, shift -= 8, idx++)
                i1 |= bytes[idx] << shift;

            for (int i = 0, shift = lowShift; i < lowIterations; i++, shift -= 8, idx++)
                i2 |= bytes[idx] << shift;

            return (uint)i2 | ((long)i1 << 32);
        }

        public string ReadString(int len = 4)
        {
            int stringLength = ReadInt32(len);
            return Utf8.Convert(ReadBytes(stringLength));
        }

        public string ReadTz1Address()
        {
            return ReadBase58(Lengths.tz1.Decoded, Prefix.tz1);
        }

        public T ReadEnumerableSingle<T>(Func<MichelineReader, T> readData)
        {
            if (!EndOfStream)
            {
                int arrLen = ReadInt32();

                byte[] arrData = ReadBytes(arrLen);

                using (MichelineReader reader = new MichelineReader(arrData))
                {
                    T result = readData(reader);

                    if (!reader.EndOfStream)
                    {
                        throw new InvalidOperationException("Expected end of stream but not reached");
                    }

                    return result;
                }
            }
            return default(T);
        }

        public IEnumerable<T> ReadEnumerable<T>(Func<MichelineReader, T> readData)
        {
            if (!EndOfStream)
            {
                int arrLen = ReadInt32();

                byte[] arrData = ReadBytes(arrLen);

                using (MichelineReader reader = new MichelineReader(arrData))
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
            int stringLength = ReadInt32();
            return Hex.Convert(ReadBytes(stringLength));
        }

        public string ReadBase58(int length, byte[] prefix = null)
        {
            byte[] b58bytes = ReadBytes(length);

            return prefix == null ?
                Base58.Convert(b58bytes) :
                Base58.Convert(b58bytes, prefix);
        }

        private byte[] ReadArrayData()
        {
            int arrLen = ReadInt32();
            return ReadBytes(arrLen);
        }
    }
}
