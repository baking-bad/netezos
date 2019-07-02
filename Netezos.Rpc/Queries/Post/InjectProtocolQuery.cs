using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class InjectProtocolQuery : RpcPost
    {
        internal InjectProtocolQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }
        /// <summary>Inject protocol</summary>
        /// <param name="expectedEnvVersion">expected environment version</param>
        /// <param name="components">Components</param>
        /// <param name="async">Async</param>
        /// <param name="force">Force</param>
        /// <returns></returns>
        public async Task<JToken> PostAsync(int expectedEnvVersion, List<object> components, bool async = false, bool force = false)
            => await Client.Post(
                $"{Query}?async={async}&force={force}",
                new
                {
                    expected_env_version = expectedEnvVersion,
                    components
                }.ToJson());
    }
}