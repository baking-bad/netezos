using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Netezos.Keys
{
    /// <summary>
    /// 
    /// </summary>
    public class HDPath : IEnumerable<uint>
    {
        /// <summary>
        /// True if the last index in the path is hardened
        /// </summary>
        public bool Hardened => (Indexes.LastOrDefault() & 0x80000000) != 0;

        readonly uint[] Indexes;

        /// <summary>
        /// 
        /// </summary>
        public HDPath()
        {
            Indexes = Array.Empty<uint>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public HDPath(string path)
        {
            path = path?.TrimStart('m').Trim('/')
                ?? throw new ArgumentNullException(nameof(path));
            Indexes = path.Length == 0
                ? Array.Empty<uint>()
                : path.Split('/').Select(ParseIndex).ToArray();
        }

        HDPath(uint[] indexes)
        {
            Indexes = indexes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hardened"></param>
        /// <returns></returns>
        public HDPath Derive(int index, bool hardened = false)
        {
            var indexes = new uint[Indexes.Length + 1];
            Indexes.CopyTo(indexes, 0);
            indexes[indexes.Length - 1] = GetIndex(index, hardened);
            return new(indexes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Indexes.Length == 0) return "m";
            return $"m/{string.Join("/", Indexes.Select(IndexToString))}";
        }

        #region IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<uint> GetEnumerator() => ((IEnumerable<uint>)Indexes).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Indexes.GetEnumerator();
        #endregion

        #region static
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static HDPath Parse(string path)
        {
            return new(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public static bool TryParse(string path, out HDPath res)
        {
            res = null;
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

            var hardened = str[str.Length - 1] == '\'' || str[str.Length - 1] == 'h';

            if (!uint.TryParse(hardened ? str.Substring(0, str.Length - 1) : str, out ind))
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

            var hardened = str[str.Length - 1] == '\'' || str[str.Length - 1] == 'h';

            if (!uint.TryParse(hardened ? str.Substring(0, str.Length - 1) : str, out var ind))
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