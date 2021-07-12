namespace Netezos.Rpc.Queries
{
    public class ConfigQuery : RpcObject
    {
        /// <summary>
        /// List of protocols which replace other protocols
        /// </summary>
        public RpcObject UserActivatedProtocolOverrides => new RpcObject(this, "user_activated_protocol_overrides/");

        /// <summary>
        /// List of protocols to switch to at given levels
        /// </summary>
        public RpcObject UserActivatedUpgrades => new RpcObject(this, "user_activated_upgrades/");

        internal ConfigQuery(RpcClient client, string query) : base(client, query) { }
    }
}