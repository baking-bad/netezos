using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Netezos.Keys
{
    public class HDPath
    {
        public int Depth => _Indexes.Length;

        public IEnumerable<uint> Indexes => _Indexes;

        readonly uint[] _Indexes;

        public HDPath()
        {
            _Indexes = new uint[0];
        }
        
        public HDPath(string path)
        {
	        int count = 0;
	        _Indexes =
		        path
			        .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
			        .Where(p => p != "m")
			        .Select(p =>
			        {
				        if (!TryParseCore(p, out var i))
					        throw new FormatException("HDPath uncorrectly formatted");
				        count++;
				        if (count > 255)
					        throw new FormatException("HDPath uncorrectly formatted");
				        return i;
			        })
			        .ToArray();
        }
        
        public HDPath(uint[] indexes)
        {
	        if (indexes.Length > 255)
		        throw new ArgumentException(paramName: nameof(indexes), message: "An HDPath should have at most 255 indices");
	        _Indexes = indexes;
        }

        public HDPath Derive(HDPath path)
        {
            //TODO: appended the path and return the new path
            throw new NotImplementedException();
        }

        public HDPath Derive(int index, bool hardened = false)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// True if the last index in the path is hardened
        /// </summary>
        public bool IsHardened
        {
	        get
	        {
		        if (_Indexes.Length == 0)
			        throw new InvalidOperationException("No index found in this HDPath");
		        return (_Indexes[_Indexes.Length - 1] & 0x80000000u) != 0;
	        }
        }

        string? _Path;
        public override string ToString()
        {
	        return _Path ?? (_Path = string.Join("/", _Indexes.Select(ToString).ToArray()));
        }
        
        private static string ToString(uint i)
        {
	        var hardened = (i & 0x80000000u) != 0;
	        var nonhardened = (i & ~0x80000000u);
	        return hardened ? nonhardened + "'" : nonhardened.ToString(CultureInfo.InvariantCulture);
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
			bool isValid = true;
			int count = 0;
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
			bool hardened = i[i.Length - 1] == '\'' || i[i.Length - 1] == 'h';
			var nonhardened = hardened ? i.Substring(0, i.Length - 1) : i;
			if (!uint.TryParse(nonhardened, out index))
				return false;

			// when parsing, number equals or greater than 0x80000000 (= 2147483648) should not be allowed.
			if (index >= 0x80000000u)
			{
				index = 0;
				return false;
			}
			if (hardened)
			{
				index = index |  0x80000000u;
				return true;
			}
			else
			{
				return true;
			}
		}
        
        private static bool IsValidPath(string path)
        {
            var regex = new Regex("^m(\\/[0-9]+')+$");

            if (!regex.IsMatch(path))
                return false;

            var valid = !(path.Split('/')
                .Slice(1)
                .Select(a => a.Replace("'", ""))
                .Any(a => !Int32.TryParse(a, out _)));

            return valid;
        }
        
        #endregion
    }
}
