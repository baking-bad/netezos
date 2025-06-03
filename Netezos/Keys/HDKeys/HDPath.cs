using System.Collections;

namespace Netezos.Keys
{
    /// <summary>
    /// Represents a path in the HD keys hierarchy (BIP-32)
    /// </summary>
    public class HDPath : IEnumerable<uint>
    {
        /// <summary>
        /// True if the last index in the path is hardened
        /// </summary>
        public bool Hardened => (Indexes.LastOrDefault() & 0x80000000) != 0;

        readonly uint[] Indexes;

        /// <summary>
        /// Creates an empty (root) HDPath object
        /// </summary>
        public HDPath()
        {
            Indexes = [];
        }

        /// <summary>
        /// Creates an HDPath object from a given string path.
        /// </summary>
        /// <param name="path">Path string, formatted like m/44'/1729'/0/0'</param>
        public HDPath(string path)
        {
            path = path?.TrimStart('m').Trim('/')
                ?? throw new ArgumentNullException(nameof(path));
            Indexes = path.Length == 0 ? [] : [..path.Split('/').Select(ParseIndex)];
        }

        HDPath(uint[] indexes)
        {
            Indexes = indexes;
        }

        /// <summary>
        /// Returns a new HDPath object with appended child index.
        /// </summary>
        /// <param name="index">Child index</param>
        /// <param name="hardened">If true, hardened derivation will be performed</param>
        /// <returns>HDPath object</returns>
        public HDPath Derive(int index, bool hardened = false)
        {
            var indexes = new uint[Indexes.Length + 1];
            Indexes.CopyTo(indexes, 0);
            indexes[^1] = GetIndex(index, hardened);
            return new(indexes);
        }

        /// <summary>
        /// Converts the HDPath to a string, formatted like m/44'/1729'/0/0'
        /// </summary>
        /// <returns>HDPath string, formatted like m/44'/1729'/0/0'</returns>
        public override string ToString()
        {
            return Indexes.Length == 0 ? "m" : $"m/{string.Join("/", Indexes.Select(IndexToString))}";
        }

        #region IEnumerable
        /// <summary>Returns an enumerator that iterates through an HDPath indexes collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the HDPath indexes collection.</returns>
        public IEnumerator<uint> GetEnumerator() => ((IEnumerable<uint>)Indexes).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Indexes.GetEnumerator();
        #endregion

        #region static
        /// <summary>
        /// Converts the path string, formatted like m/44'/1729'/0/0', to the HDPath object
        /// </summary>
        /// <param name="path">HD key path string, formatted like m/44'/1729'/0/0'</param>
        /// <returns>HDPath object</returns>
        public static HDPath Parse(string path)
        {
            return new(path);
        }

        /// <summary>
        /// Converts the path string, formatted like m/44'/1729'/0/0', to the HDPath object
        /// </summary>
        /// <param name="path">HD key path string, formatted like m/44'/1729'/0/0'</param>
        /// <param name="res">Successfully parsed HDPath</param>
        /// <returns>True if the HDPath is parsed successfully, otherwise false</returns>
        public static bool TryParse(string? path, out HDPath res)
        {
            res = null!;
            if (path == null) return false;

            path = path.TrimStart('m').Trim('/');
            if (path.Length == 0)
            {
                res = new();
                return true;
            }

            var ss = path.Split('/');
            var indexes = new uint[ss.Length];
            for (int i = 0; i < ss.Length; i++)
            {
                if (!TryParseIndex(ss[i], out var ind))
                    return false;
                indexes[i] = ind;
            }

            res = new(indexes);
            return true;
        }

        internal static uint GetIndex(int index, bool hardened)
        {
            if (index < 0)
                throw new ArgumentException("Index must be positive", nameof(index));

            return hardened ? (uint)index | 0x80000000 : (uint)index;
        }

        static bool TryParseIndex(string str, out uint ind)
        {
            if (str.Length == 0)
            {
                ind = 0;
                return false;
            }

            var hardened = str[^1] == '\'' || str[^1] == 'h';

            if (!uint.TryParse(hardened ? str[..^1] : str, out ind))
                return false;

            if ((ind & 0x80000000) != 0)
                return false;

            if (hardened)
                ind |= 0x80000000;

            return true;
        }

        static uint ParseIndex(string str)
        {
            if (str.Length == 0)
                throw new FormatException("Path contains empty element");

            var hardened = str[^1] == '\'' || str[^1] == 'h';

            if (!uint.TryParse(hardened ? str[..^1] : str, out var ind))
                throw new FormatException("Path contains invalid index");

            if ((ind & 0x80000000) != 0)
                throw new FormatException("Path contains too large index");

            return hardened ? (ind | 0x80000000) : ind;
        }

        static string IndexToString(uint ind)
        {
            var plain = ind & 0x7FFFFFFF;
            var hardened = (ind & 0x80000000) != 0;
            return hardened ? $"{plain}'" : plain.ToString();
        }
        #endregion
    }
}