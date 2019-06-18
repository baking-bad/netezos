using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
//TODO Summary
namespace Netezos.Rpc
{
    public class RpcPostArgs
    {
        protected Dictionary<string, object> Args = new Dictionary<string, object>();

        public override string ToString() => JsonConvert.SerializeObject(Args, new JsonSerializerSettings { 
            NullValueHandling = NullValueHandling.Ignore
        });
        public void Add(object key, string type, string prim)
        {
            Args["key"] = new Dictionary<string, object>
            {
                {type, key}
            };
            Args["type"] = new Dictionary<string, object>
            {
                {"prim", prim}
            };
        }

        public void Add(string key, object value)
        {
            Args[key] = value;
        }



        public void AddBytes(string key, string prim) => Add(key, "bytes", prim);
        public void AddBytes(string key) =>  AddBytes(key, "bytes");
        public void AddBytes(byte[] key, string prim) => AddBytes(BitConverter.ToString(key).Replace("-", ""), prim);
        public void AddBytes(byte[] key) => AddBytes(BitConverter.ToString(key).Replace("-", ""));
        public void AddString(string key, string prim) => Add(key, "string", prim);
        public void AddInt(int key, string prim) => Add(key, "int", prim);
        public void AddInt(int key) => AddInt(key, "int");
    }
}