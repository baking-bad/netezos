using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netezos.Keys
{
    /// <summary>
    /// Represent a path in the hierarchy of HD keys (BIP32).
    /// </summary>
    public class HDPath
    {
        private const uint IntOverflow = 0x80000000U;
        private const byte ByteMax = 255;

        private static readonly string Slash = "/";
        private static readonly char[] SlashChar = new char[] { Slash[0] };

        private readonly uint[] _indexes;
        private string _path;

        /// <summary>
        /// Represents an empty <see cref="HDPath"/>.
        /// </summary>
        public static HDPath Empty { get; } = new HDPath(new uint[0]);

        /// <summary>
        /// True if at least one index in the path is hardened; otherwise, false.
        /// </summary>
        public bool IsHardenedPath => _indexes.Any(i => (i & IntOverflow) != 0);

        /// <summary>
        /// True if the last index in the path is hardened; otherwise, false.
        /// </summary>
        public bool IsHardened => (GetPathIndexValue(_indexes.Length - 1) & IntOverflow) != 0;

        public int Length => _indexes.Length;

        public uint this[int index] => _indexes[index];

        public uint[] Indexes => _indexes.ToArray();

        public HDPath()
        {
            _indexes = new uint[0];
        }

        public HDPath(params uint[] indexes)
        {
            if (indexes.Length > ByteMax)
            {
                throw new ArgumentException($"{nameof(HDPath)} should have at most 255 indices.", nameof(indexes));
            }
            _indexes = indexes;
        }

        public HDPath(string path)
        {
            int count = 0;

            _indexes =
                path
                .Split(SlashChar, StringSplitOptions.RemoveEmptyEntries)
                .Where(p => p != "m")
                .Select(p =>
                {
                    if (!TryParseCore(p, out uint index))
                    {
                        throw new FormatException($"{nameof(HDPath)} incorrectly formatted.");
                    }

                    count++;

                    if (count > ByteMax)
                    {
                        throw new FormatException($"{nameof(HDPath)} incorrectly formatted.");
                    }

                    return index;
                })
                .ToArray();
        }

        public override bool Equals(object obj) => (obj is HDPath k) && _indexes.Length == k._indexes.Length && _indexes.SequenceEqual(k._indexes);

        public override int GetHashCode() => ToString().GetHashCode();

        public override string ToString() => _path ?? (_path = string.Join(Slash, _indexes.Select(ToString).ToArray()));

        /// <summary>
        /// Returns the longest non-hardened HDPath to the leaf.
        /// For example, if the HDPath is "44'/1729'/0'/1/23", then the address key path is "1/23".
        /// </summary>
        /// <returns>Return the address key path.</returns>
        public HDPath GetAddressHDPath()
        {
            List<uint> indexes = new List<uint>();

            for (int i = Indexes.Length - 1; i >= 0; i--)
            {
                if (Indexes[i] >= IntOverflow)
                {
                    break;
                }

                indexes.Insert(0, Indexes[i]);
            }

            return new HDPath(indexes.ToArray());
        }

        /// <summary>
        /// Returns the longest hardened HDPath from the root.
        /// For example, if the HDPath is "44'/1729'/0'/1/23", then the account key path is "44'/1729'/0'"
        /// </summary>
        /// <returns>Return the account key path.</returns>
        public HDPath GetAccountHDPath()
        {
            List<uint> indexes = new List<uint>();

            for (int i = 0; i < Indexes.Length; i++)
            {
                if (Indexes[i] < IntOverflow)
                {
                    break;
                }

                indexes.Add(Indexes[i]);
            }

            return new HDPath(indexes.ToArray());
        }

        public byte[] ToBytes()
        {
            return Indexes.Count() == 0 ?
                new byte[0] :
                Indexes.Select(i => ToBytes(i)).Aggregate((a, b) => a.Concat(b)).ToArray();
        }

        private uint GetPathIndexValue(int index)
        {
            if (_indexes.Length == 0)
            {
                throw new InvalidOperationException($"No index found in {nameof(HDPath)}.");
            }
            return _indexes[index] & IntOverflow;
        }

        public HDPath Derive(int index, bool hardened)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"{nameof(index)} must not be negative.");
            }

            uint realIndex = (uint)index;
            realIndex = hardened ? realIndex | IntOverflow : realIndex;

            return Derive(new HDPath(realIndex));
        }

        public HDPath Derive(string path) => Derive(new HDPath(path));

        public HDPath Derive(uint index) => Derive(new HDPath(index));

        public HDPath Derive(HDPath derivation)
        {
            return new HDPath
            (
                _indexes
                .Concat(derivation._indexes)
                .ToArray()
            );
        }

        public HDPath Parent
        {
            get
            {
                if (_indexes.Length == 0)
                {
                    return null;
                }
                return new HDPath(_indexes.Take(_indexes.Length - 1).ToArray());
            }
        }

        public HDPath Increment()
        {
            if (_indexes.Length == 0)
            {
                throw new InvalidOperationException($"Cannot increment an empty {nameof(HDPath)}.");
            }

            uint[] indices = _indexes.ToArray();

            indices[indices.Length - 1]++;

            return new HDPath(indices);
        }

        /// <summary>
        /// Parse a HDPath
        /// </summary>
        /// <param name="path">The HDPath formated like 10/0/2'/3</param>
        /// <returns></returns>
        public static HDPath Parse(string path) => new HDPath(path);

        /// <summary>
        /// Converts the string representation of an HD key path to its <see cref="HDPath"/> equivalent.
        /// A return value indicates whether the operation succeeded.
        /// </summary>
        /// <param name="path">The HDPath formated like 44'/1729'/0'/0</param>
        /// <param name="HDPath">The successfully parsed <see cref="HDPath"/>.</param>
        /// <returns>True if the string is parsed successfully; otherwise false</returns>
        public static bool TryParse(string path, out HDPath HDPath)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            bool valid = true;
            int count = 0;

            uint[] indices = path
                .Split(SlashChar, StringSplitOptions.RemoveEmptyEntries)
                .Where(c => c != "m")
                .Select(c =>
                {
                    valid &= TryParseCore(c, out var i);
                    count++;

                    if (count > 255)
                    {
                        valid = false;
                    }

                    return i;
                })
                .Where(_ => valid)
                .ToArray();

            if (!valid)
            {
                HDPath = null;
                return false;
            }

            HDPath = new HDPath(indices);

            return true;
        }

        private static bool TryParseCore(string input, out uint index)
        {
            if (input.Length == 0)
            {
                index = 0;
                return false;
            }

            bool hardened = input[input.Length - 1] == '\'' || input[input.Length - 1] == 'h';

            string nonhardened = hardened ?
                input.Substring(0, input.Length - 1) :
                input;

            if (!uint.TryParse(nonhardened, out index))
            {
                return false;
            }

            if (index >= IntOverflow)
            {
                index = 0;
                return false;
            }
            else if (hardened)
            {
                index = index | IntOverflow;
                return true;
            }
            else
            {
                return true;
            }
        }

        public static HDPath FromBytes(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            else if (data.Length % 4 != 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(data)} length is not valid for {nameof(HDPath)}");
            }

            int depth = data.Length / 4;

            uint[] result = new uint[depth];

            for (int i = 0; i < depth; i++)
            {
                result[i] = ToUInt32(data, i * 4);
            }

            return new HDPath(result);
        }

        private static string ToString(uint i)
        {
            bool hardened = (i & IntOverflow) != 0;

            uint nonhardened = (i & ~IntOverflow);

            return hardened ? $"{nonhardened}'" : nonhardened.ToString(CultureInfo.InvariantCulture);
        }

        private static uint ToUInt32(byte[] value, int index)
        {
            return value[index]
                + ((uint)value[index + 1] << 8)
                + ((uint)value[index + 2] << 16)
                + ((uint)value[index + 3] << 24);
        }

        private static byte[] ToBytes(ulong value)
        {
            return new byte[]
            {
                (byte)value,
                (byte)(value >> 8),
                (byte)(value >> 16),
                (byte)(value >> 24),
                (byte)(value >> 32),
                (byte)(value >> 40),
                (byte)(value >> 48),
                (byte)(value >> 56),
            };
        }

        public static bool operator ==(HDPath a, HDPath b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            else if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            else
            {
                return a.ToString() == b.ToString();
            }
        }

        public static HDPath operator +(HDPath a, HDPath b)
        {
            if (a is null && !(b is null))
            {
                return b;
            }
            else if (b is null && !(a is null))
            {
                return a;
            }
            else if (a is null && b is null)
            {
                return null;
            }
            else
            {
                return a.Derive(b);
            }
        }

        public static bool operator !=(HDPath a, HDPath b)
        {
            return !(a == b);
        }
    }
}