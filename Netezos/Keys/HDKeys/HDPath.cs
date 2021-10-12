using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Netezos.Keys
{
    public class HDPath
    {
        public IEnumerable<uint> Indexes { get; }

        public HDPath()
        {
            Indexes = Array.Empty<uint>();
        }

        public HDPath(string path)
        {
            var count = 0;
            Indexes = path
                .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(p => p != "m")
                .Select(p =>
                {
                    if (!TryParseCore(p, out var i))
                        throw new FormatException("HDPath incorrectly formatted");
                    count++;
                    if (count > 255)
                        throw new FormatException("HDPath incorrectly formatted");
                    return i;
                })
                .ToArray();
        }

        public HDPath(params uint[] indexes)
        {
            if (indexes.Length > 255)
                throw new ArgumentException(paramName: nameof(indexes),
                    message: "An HDPath should have at most 255 indices");
            Indexes = indexes;
        }

        public HDPath Derive(HDPath path)
        {
            return new HDPath(
                Indexes
                    .Concat(path.Indexes)
                    .ToArray());
        }

        public HDPath Derive(int index, bool hardened = false)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "the index can't be negative");
            var realIndex = (uint)index;
            realIndex = hardened ? realIndex | 0x80000000u : realIndex;
            return Derive(new HDPath(realIndex));
        }

        /// <summary>
        /// True if the last index in the path is hardened
        /// </summary>
        public bool IsHardened
        {
            get
            {
                if (!Indexes.Any())
                    throw new InvalidOperationException("No index found in this HDPath");
                return (Indexes.Last() & 0x80000000u) != 0;
            }
        }

        string Path;

        public override string ToString()
        {
            return Path ??= string.Join("/", Indexes.Select(ToString).ToArray());
        }

        private static string ToString(uint i)
        {
            var hardened = (i & 0x80000000u) != 0;
            var unhardened = (i & ~0x80000000u);
            return hardened ? unhardened + "'" : unhardened.ToString(CultureInfo.InvariantCulture);
        }

        #region static

        public static HDPath Parse(string path)
        {
            return new HDPath(path);
        }

        public static bool TryParse(string path, out HDPath keyPath)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            var isValid = true;
            var count = 0;
            var indices =
                path
                    .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(p => p != "m")
                    .Select(p =>
                    {
                        isValid &= TryParseCore(p, out var i);
                        count++;
                        if (count > 255)
                            isValid = false;
                        return i;
                    })
                    .Where(_ => isValid)
                    .ToArray();
            if (!isValid)
            {
                keyPath = null;
                return false;
            }

            keyPath = new HDPath(indices);
            return true;
        }

        private static bool TryParseCore(string i, out uint index)
        {
            if (i.Length == 0)
            {
                index = 0;
                return false;
            }

            var hardened = i[i.Length - 1] == '\'' || i[i.Length - 1] == 'h';
            var unhardened = hardened ? i.Substring(0, i.Length - 1) : i;
            if (!uint.TryParse(unhardened, out index))
                return false;

            // when parsing, number equals or greater than 0x80000000 (= 2147483648) should not be allowed.
            if (index >= 0x80000000u)
            {
                index = 0;
                return false;
            }

            if (hardened)
            {
                index |= 0x80000000u;
                return true;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}