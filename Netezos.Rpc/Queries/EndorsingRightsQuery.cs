using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to get endorsing rights
    /// </summary>
    public class EndorsingRightsQuery : RpcObject
    {
        internal EndorsingRightsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <returns></returns>
        public new async Task<JToken> GetAsync()
            => await Client.GetJson(Query);

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="baker">Delegate whose endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<JToken> GetAsync(string baker)
            => await Client.GetJson($"{Query}?delegate={baker}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="level">Level of the block at which the endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<JToken> GetFromLevelAsync(int level)
            => await Client.GetJson($"{Query}?level={level}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="level">Level of the block at which the endorsing rights are to be returned</param>
        /// <param name="baker">Delegate whose endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<JToken> GetFromLevelAsync(int level, string baker)
            => await Client.GetJson($"{Query}?level={level}&delegate={baker}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="cycle">Cycle at which the endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<JToken> GetFromCycleAsync(int cycle)
            => await Client.GetJson($"{Query}?cycle={cycle}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="cycle">Cycle at which the endorsing rights are to be returned</param>
        /// <param name="baker">Delegate whose endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<JToken> GetFromCycleAsync(int cycle, string baker)
            => await Client.GetJson($"{Query}?cycle={cycle}&delegate={baker}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <returns></returns>
        public new async Task<T> GetAsync<T>()
            => await Client.GetJson<T>(Query);

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="baker">Delegate whose endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string baker)
            => await Client.GetJson<T>($"{Query}?delegate={baker}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="level">Level of the block at which the endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<T> GetFromLevelAsync<T>(int level)
            => await Client.GetJson<T>($"{Query}?level={level}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="level">Level of the block at which the endorsing rights are to be returned</param>
        /// <param name="baker">Delegate whose endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<T> GetFromLevelAsync<T>(int level, string baker)
            => await Client.GetJson<T>($"{Query}?level={level}&delegate={baker}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="cycle">Cycle at which the endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<T> GetFromCycleAsync<T>(int cycle)
            => await Client.GetJson<T>($"{Query}?cycle={cycle}");

        /// <summary>
        /// Executes the query and returns the endorsing rights
        /// </summary>
        /// <param name="cycle">Cycle at which the endorsing rights are to be returned</param>
        /// <param name="baker">Delegate whose endorsing rights are to be returned</param>
        /// <returns></returns>
        public async Task<T> GetFromCycleAsync<T>(int cycle, string baker)
            => await Client.GetJson<T>($"{Query}?cycle={cycle}&delegate={baker}");
    }
}
