using System;
using System.Collections.Generic;

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
            //TODO: parse path from string like "m/0/1'/2"
            throw new NotImplementedException();
        }

        public HDPath Derive(HDPath path)
        {
            //TODO: appended the path and return the new path
            throw new NotImplementedException();
        }

        public HDPath Derive(int index, bool hardened = false)
        {
            //TODO: appended the index and return the new path
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
            //TODO: parse path from string like "m/0/1'/2"
            throw new NotImplementedException();
        }

        public static bool TryParse(string path, out HDPath result)
        {
            //TODO: parse path from string like "m/0/1'/2"
            throw new NotImplementedException();
        }
        #endregion
    }
}
