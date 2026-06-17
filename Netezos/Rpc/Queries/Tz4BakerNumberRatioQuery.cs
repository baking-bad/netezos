namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to get the ratio of active bakers using a tz4
    /// </summary>
    public class Tz4BakerNumberRatioQuery : RpcObject
    {
        /// <summary>
        /// Returns the ratio of active bakers using a tz4 for the current cycle
        /// </summary>
        public new Task<dynamic> GetAsync()
            => Client.GetJson(Query);

        /// <summary>
        /// Returns the ratio of active bakers using a tz4 for the given cycle
        /// </summary>
        /// <param name="cycle">Cycle number</param>
        public Task<dynamic> GetAsync(int cycle)
            => Client.GetJson($"{Query}?cycle={cycle}");

        /// <summary>
        /// Returns the ratio of active bakers using a tz4 for the given cycle
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="cycle">Cycle number</param>
        public Task<T?> GetAsync<T>(int cycle)
            => Client.GetJson<T>($"{Query}?cycle={cycle}");

        internal Tz4BakerNumberRatioQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
