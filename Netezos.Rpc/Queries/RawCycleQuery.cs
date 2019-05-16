namespace Netezos.Rpc.Queries
{
    public class RawCycleQuery : DeepRpcObject
    {
        public DeepRpcDictionary<int, RpcObject> LastRoll
            => new DeepRpcDictionary<int, RpcObject>(this, "last_roll/");

        /// <summary>
        /// Warning: shouldn't be used to get all the nonces
        /// </summary>
        public DeepRpcDictionary<int, RpcObject> Nonces
            => new DeepRpcDictionary<int, RpcObject>(this, "nonces/");

        public RpcObject RandomSeed => new RpcObject(this, "random_seed/");

        public RpcObject RollSnapshot => new RpcObject(this, "roll_snapshot/");

        internal RawCycleQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
