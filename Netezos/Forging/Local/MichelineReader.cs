﻿using Netezos.Encoding;
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
            byte tag = _reader.ReadByte();

            switch (tag)
            {
                case 0: return ReadInt();
                case 1: return ReadString();
                case 2: return ReadArray();
                case 3: return ReadPrimitive(0, false);
                case 4: return ReadPrimitive(0, true);
                case 5: return ReadPrimitive(1, false);
                case 6: return ReadPrimitive(1, true);
                case 7: return ReadPrimitive(2, false);
                case 8: return ReadPrimitive(2, true);
                case 9: return ReadPrimitive(3, true);
                case 10: return ReadBytes();
                default: throw new InvalidOperationException($"Unknown tag {tag} at position {_reader.BaseStream.Position}");
            }
        }

        public MichelineInt ReadInt()
        {
            return new MichelineInt(ReadMicheInt());
        }

        public MichelineString ReadString()
        {
            return new MichelineString(ReadNativeString());
        }

        public MichelineArray ReadArray()
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

        private MichelinePrim ReadPrimitive(int argsLength, bool annotations = false)
        {
            PrimType primTag = (PrimType)_reader.ReadByte();

            MichelinePrim prim = new MichelinePrim
            {
                Prim = primTag
            };

            if (0 < argsLength && argsLength < 3)
            {
                for (int i = 0; i < argsLength; i++)
                {
                    prim.Args.Add(ReadMicheline());
                }
            }
            else if (argsLength == 3)
            {
                prim.Args = ReadArray();
            }
            else
            {
                throw new ArgumentException($"Unexpected args length {argsLength}", nameof(argsLength));
            }

            if (annotations)
            {
                byte[] value = ReadArrayData();

                if (value.Length > 0)
                {
                    using (MichelineReader valueReader = new MichelineReader(value))
                    {
                        prim.Annots = valueReader.ReadNativeString()
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
            }

            return prim;
        }

        public MichelineBytes ReadBytes()
        {
            return new MichelineBytes(ReadArrayData());
        }

        public string ReadPublicKey()
        {
            byte[] prefix;

            byte id = _reader.ReadByte();

            switch (id)
            {
                case 0: prefix = Prefix.edpk; break;
                case 1: prefix = Prefix.sppk; break;
                case 2: prefix = Prefix.p2pk; break;
                default: throw new ArgumentException($"Invalid public key prefix {id}");
            };

            return Base58.Convert(_reader.ReadBytes(32), prefix);
        }

        public string ReadAddress()
        {
            byte type = _reader.ReadByte();

            switch (type)
            {
                case 0: return ReadTzAddress();
                case 1: return ReadKtAddress();
                default: throw new ArgumentException($"Invalid address prefix {type}");
            }
        }

        private string ReadTzAddress()
        {
            byte[] prefix;

            byte tzType = _reader.ReadByte();

            switch (tzType)
            {
                case 0: prefix = Prefix.tz1; break;
                case 1: prefix = Prefix.tz2; break;
                case 2: prefix = Prefix.tz3; break;
                default:
                    throw new ArgumentException($"Invalid source prefix {tzType}");
            }

            return Base58.Convert(_reader.ReadBytes(33), prefix);
        }

        private string ReadKtAddress()
        {
            _reader.ReadByte(); // Consume padded 0
            return Base58.Convert(_reader.ReadBytes(33), Prefix.KT1);
        }

        public int ReadMicheNat()
        {
            int value = 0;

            List<byte> bytes = new List<byte>();

            byte b;
            while (((b = _reader.ReadByte()) & 0x80) != 0)
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

        public BigInteger ReadMicheInt()
        {
            int value = 0;

            List<byte> bytes = new List<byte>();

            byte b;
            while (((b = _reader.ReadByte()) & 0x80) != 0)
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
            byte epType = _reader.ReadByte();

            switch (epType)
            {
                case 0: return "default";
                case 1: return "root";
                case 2: return "do";
                case 3: return "set_delegate";
                case 4: return "remove_delegate";
                case 255: return ReadNativeString();
                default: throw new ArgumentException($"Invalid entrypoint prefix {epType}");
            }
        }

        //public string ReadTz1Address(byte[] bytes)
        //{
        //    return Base58.Convert(bytes.GetBytes(1, bytes.Length - 1), Prefix.tz1);
        //}

        private byte[] ReadArrayData()
        {
            int arrLen = ReadNativeInt32();
            return _reader.ReadBytes(arrLen);
        }

        private bool ReadNativeBool()
        {
            return _reader.ReadByte() == 255;
        }

        private int ReadNativeInt32()
        {
            byte[] bytes = _reader.ReadBytes(4);

            int res = 0;

            for (int i = 0, shift = 24; i < 4; i++, shift -= 8)
            {
                res |= bytes[i] << shift;
            }

            return res;
        }

        private long ReadNativeInt64()
        {
            byte[] bytes = _reader.ReadBytes(8);

            var idx = 0;

            var i1 = 0;
            for (int i = 0, shift = 24; i < 4; i++, shift -= 8, idx++)
                i1 |= bytes[idx] << shift;

            var i2 = 0;
            for (int i = 0, shift = 24; i < 4; i++, shift -= 8, idx++)
                i2 |= bytes[idx] << shift;

            return (uint)i2 | ((long)i1 << 32);
        }

        private string ReadNativeString()
        {
            int stringLength = ReadNativeInt32();
            return Utf8.Convert(_reader.ReadBytes(stringLength));
        }
    }
}
