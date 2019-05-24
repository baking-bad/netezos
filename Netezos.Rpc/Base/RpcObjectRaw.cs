using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to get a json object
    /// </summary>
    public class RpcObjectRaw : RpcObject
    {
        /// <summary>
        /// Gets the query to the raw (unparsed) data
        /// </summary>
        public RpcObject Raw => new RpcObject(this, "raw/");

        internal RpcObjectRaw(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        public override string ToString() => Query;
    }
}
