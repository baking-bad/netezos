using System.Threading.Tasks;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access normalized output
    /// </summary>
    public class NormalizedQuery : RpcMethod
    {
        internal NormalizedQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        /// Normalize the output using the requested unparsing mode.
        /// </summary>
        /// <param name="mode">Unparsing mode</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(UnparsingMode mode)
            => PostAsync(new
            {
                unparsing_mode = mode.ToString()
            });

        public enum UnparsingMode
        {
            Readable,
            Optimized,
            Optimized_legacy
        }
    }
}