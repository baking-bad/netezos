using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access the normalized big_map
    /// </summary>
    public class NormalizedBigMapQuery : RpcMethod
    {
        internal NormalizedBigMapQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Access the value associated with a key in a big map, normalize the output using the requested unparsing mode.
        /// </summary>
        /// <param name="unparsingMode">Unparsing mode. `Readable`, `Optimized` or `Optimized_legacy`</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(BigMapNormalization unparsingMode)
            => PostAsync(new
            {
                unparsing_mode = unparsingMode.ToString()
            });
    }
}