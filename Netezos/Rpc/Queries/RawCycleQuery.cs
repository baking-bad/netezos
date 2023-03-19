namespace Netezos.Rpc.Queries
{
    public class RawCycleQuery : DeepRpcObject
    {
        public DeepRpcDictionary<int, RpcObject> LastRoll => new(this, "last_roll/");

        /// <summary>
        /// Warning: shouldn't be used to get all the nonces
        /// </summary>
        public DeepRpcDictionary<int, RpcObject> Nonces => new(this, "nonces/");

        public RpcObject RandomSeed => new(this, "random_seed/");

        public RpcObject RollSnapshot => new(this, "roll_snapshot/");

        internal RawCycleQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
