namespace Netezos.Rpc.Queries;

public class ProtocolQuery : RpcQuery
{
    /// <summary>
    /// Returns the level at which the current protocol was activated.
    /// </summary>
    public RpcObject FirstLevel => new(this, "first_level/");
        
    internal ProtocolQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}