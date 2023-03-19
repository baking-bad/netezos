namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access to a michelson script view
    /// </summary>
    public class RunScriptViewQuery : RpcMethod
    {
        internal RunScriptViewQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Simulate a call to a michelson script view
        /// </summary>
        /// <param name="contract">Contract</param>
        /// <param name="view">View</param>
        /// <param name="input">Input(micheline michelson expression)</param>
        /// <param name="chainId">Chain id</param>
        /// <param name="source">Source (optional)</param>
        /// <param name="payer">Payer (optional)</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <param name="mode">Unparsing mode</param>
        /// <param name="now">Now (optional)</param>
        /// <param name="level">Level (optional)</param>
        /// <returns></returns>
        public Task<dynamic> PostAsync(string contract, string view, object input, string chainId = "NetXdQprcVkpaWU", string? source = null,
            string? payer = null, long? gas = null, UnparsingMode mode = UnparsingMode.Readable, int? now = null, int? level = null)
            => PostAsync(new
            {
                contract,
                view,
                input,
                chain_id = chainId,
                unlimited_gas = gas == null,
                unparsing_mode = mode.ToString(),
                source,
                payer,
                gas = gas?.ToString(),
                now = now?.ToString(),
                level = level?.ToString()
            });

        /// <summary>
        /// Simulate a call to a michelson script view
        /// </summary>
        /// <param name="contract">Contract</param>
        /// <param name="view">View</param>
        /// <param name="input">Input(micheline michelson expression)</param>
        /// <param name="chainId">Chain id</param>
        /// <param name="source">Source (optional)</param>
        /// <param name="payer">Payer (optional)</param>
        /// <param name="gas">Gas limit (optional)</param>
        /// <param name="mode">Unparsing mode</param>
        /// <param name="now">Now (optional)</param>
        /// <param name="level">Level (optional)</param>
        /// <returns></returns>
        public Task<T?> PostAsync<T>(string contract, string view, object input, string chainId = "NetXdQprcVkpaWU", string? source = null,
            string? payer = null, long? gas = null, UnparsingMode mode = UnparsingMode.Readable, int? now = null, int? level = null)
            => PostAsync<T>(new
            {
                contract,
                view,
                input,
                chain_id = chainId,
                unlimited_gas = gas == null,
                unparsing_mode = mode.ToString(),
                source,
                payer,
                gas = gas?.ToString(),
                now = now?.ToString(),
                level = level?.ToString()
            });
        
        public enum UnparsingMode
        {
            Readable,
            Optimized,
            Optimized_legacy
        }
    }
}
