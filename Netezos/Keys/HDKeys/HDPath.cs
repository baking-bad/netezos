using System;
using System.Collections.Generic;
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
            if (!IsValidPath(path))
                throw new FormatException("Invalid derivation path");
            
            _Indexes = path
                .Split('/')
                .Slice(1)
                .Select(a => a.Replace("'", ""))
                .Select(a => Convert.ToUInt32(a, 10))
                .ToArray();
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

        public override string ToString()
        {
            //TODO: format to string like "m/0/1'/2"
            throw new NotImplementedException();
        }

        #region static
        public static HDPath Parse(string path)
        {
            return new HDPath(path);
        }

        public static bool TryParse(string path, out HDPath result)
        {
            //TODO: parse path from string like "m/0/1'/2"
            throw new NotImplementedException();
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
