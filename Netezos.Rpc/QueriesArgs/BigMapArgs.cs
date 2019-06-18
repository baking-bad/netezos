using System.Collections.Generic;

namespace Netezos.Rpc.QueriesArgs
{
    enum Primitives
    {
        
    }

    public class BigMapArgs : RpcPostArgs
    {
        public BigMapArgs(string key, string type)
        {
            Args["key"] = new Dictionary<string, string>
            {
                {type, key}
            };
            Args["type"] = new Dictionary<string, string>
            {
                {"prim", type}
            };
        }
        
        public BigMapArgs(int key)
        {
            Args["key"] = new Dictionary<string, object>
            {
                {"int", key}
            };
            Args["type"] = new Dictionary<string, object>
            {
                {"prim", "int"}
            };
        }

        public BigMapArgs(int key, string prim)
        {
            Args["key"] = new Dictionary<string, object>
            {
                {"int", key}
            };
            Args["type"] = new Dictionary<string, object>
            {
                {"prim", prim}
            };
        }
    }
}