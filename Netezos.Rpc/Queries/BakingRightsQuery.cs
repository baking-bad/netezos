using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to get baking rights
    /// </summary>
    public class BakingRightsQuery : RpcObject
    {
        internal BakingRightsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetAsync(bool all = false)
            => await Client.GetJson($"{Query}?all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="baker">Delegate whose baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetAsync(string baker, bool all = false)
            => await Client.GetJson($"{Query}?delegate={baker}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="maxPriority">Maximum priority of baking rights to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetAsync(int maxPriority, bool all = false)
            => await Client.GetJson($"{Query}?max_priority={maxPriority}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="level">Level of the block at which the baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetFromLevelAsync(int level, bool all = false)
            => await Client.GetJson($"{Query}?level={level}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="level">Level of the block at which the baking rights are to be returned</param>
        /// <param name="baker">Delegate whose baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetFromLevelAsync(int level, string baker, bool all = false)
            => await Client.GetJson($"{Query}?level={level}&delegate={baker}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="level">Level of the block at which the baking rights are to be returned</param>
        /// <param name="maxPriority">Maximum priority of baking rights to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetFromLevelAsync(int level, int maxPriority, bool all = false)
            => await Client.GetJson($"{Query}?level={level}&max_priority={maxPriority}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="cycle">Cycle at which the baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetFromCycleAsync(int cycle, bool all = false)
            => await Client.GetJson($"{Query}?cycle={cycle}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="cycle">Cycle at which the baking rights are to be returned</param>
        /// <param name="baker">Delegate whose baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetFromCycleAsync(int cycle, string baker, bool all = false)
            => await Client.GetJson($"{Query}?cycle={cycle}&delegate={baker}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="cycle">Cycle at which the baking rights are to be returned</param>
        /// <param name="maxPriority">Maximum priority of baking rights to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<JToken> GetFromCycleAsync(int cycle, int maxPriority, bool all = false)
            => await Client.GetJson($"{Query}?cycle={cycle}&max_priority={maxPriority}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(bool all = false)
            => await Client.GetJson<T>($"{Query}?all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="baker">Delegate whose baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string baker, bool all = false)
            => await Client.GetJson<T>($"{Query}?delegate={baker}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="maxPriority">Maximum priority of baking rights to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(int maxPriority, bool all = false)
            => await Client.GetJson<T>($"{Query}?max_priority={maxPriority}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="level">Level of the block at which the baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetFromLevelAsync<T>(int level, bool all = false)
            => await Client.GetJson<T>($"{Query}?level={level}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="level">Level of the block at which the baking rights are to be returned</param>
        /// <param name="baker">Delegate whose baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetFromLevelAsync<T>(int level, string baker, bool all = false)
            => await Client.GetJson<T>($"{Query}?level={level}&delegate={baker}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="level">Level of the block at which the baking rights are to be returned</param>
        /// <param name="maxPriority">Maximum priority of baking rights to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetFromLevelAsync<T>(int level, int maxPriority, bool all = false)
            => await Client.GetJson<T>($"{Query}?level={level}&max_priority={maxPriority}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="cycle">Cycle at which the baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetFromCycleAsync<T>(int cycle, bool all = false)
            => await Client.GetJson<T>($"{Query}?cycle={cycle}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="cycle">Cycle at which the baking rights are to be returned</param>
        /// <param name="baker">Delegate whose baking rights are to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetFromCycleAsync<T>(int cycle, string baker, bool all = false)
            => await Client.GetJson<T>($"{Query}?cycle={cycle}&delegate={baker}&all={all}");

        /// <summary>
        /// Executes the query and returns the baking rights
        /// </summary>
        /// <param name="cycle">Cycle at which the baking rights are to be returned</param>
        /// <param name="maxPriority">Maximum priority of baking rights to be returned</param>
        /// <param name="all">Specifies whether all baking opportunities for each baker will be returned or only the first one</param>
        /// <returns></returns>
        public async Task<T> GetFromCycleAsync<T>(int cycle, int maxPriority, bool all = false)
            => await Client.GetJson<T>($"{Query}?cycle={cycle}&max_priority={maxPriority}&all={all}");
    }
}
