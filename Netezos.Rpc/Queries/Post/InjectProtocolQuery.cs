using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to access protocols injection
    /// </summary>
    public class InjectProtocolQuery : RpcPost
    {
        internal InjectProtocolQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Injects a protocol into the node and returns the ID of the protocol
        /// </summary>
        /// <param name="expectedEnvVersion">Expected environment version</param>
        /// <param name="components">List of components</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="force">Force (optional)</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int expectedEnvVersion, List<object> components,bool async = false, bool force = false)
            => await Client.PostJson(
                $"{Query}?async={async}&force={force}",
                new
                {
                    expected_env_version = expectedEnvVersion,
                    components
                }.ToJson());

        /// <summary>
        /// Injects a protocol into the node and returns the ID of the protocol
        /// </summary>
        /// <param name="expectedEnvVersion">Expected environment version</param>
        /// <param name="components">List of components</param>
        /// <param name="async">Async (optional)</param>
        /// <param name="force">Force (optional)</param>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(int expectedEnvVersion, List<object> components, bool async = false, bool force = false)
            => await Client.PostJson<T>(
                $"{Query}?async={async}&force={force}",
                new
                {
                    expected_env_version = expectedEnvVersion,
                    components
                }.ToJson());
    }
}