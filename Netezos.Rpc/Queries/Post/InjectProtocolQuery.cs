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
        /// <returns></returns>
        public async Task<JToken> PostAsync(int expectedEnvVersion, List<object> components)
        {
            return await base.PostAsync(new
            {
                expected_env_version = expectedEnvVersion,
                components
            });
        }
    }
}