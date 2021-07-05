using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access the normalized big_map, script or storage
    /// </summary>
    public class NormalizedQuery : RpcMethod
    {
        internal NormalizedQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Normalize the output using the requested unparsing mode.
        /// </summary>
        /// <param name="unparsingMode">Unparsing mode. `Readable`, `Optimized` or `OptimizedLegacy`</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(Normalization unparsingMode) => PostAsync(new
            {
                unparsing_mode = unparsingMode
            });
    }
}