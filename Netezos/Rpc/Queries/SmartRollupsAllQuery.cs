namespace Netezos.Rpc.Queries;

/// <summary>
/// Rpc query to access to all smart rollups data
/// </summary>
public class SmartRollupsAllQuery : RpcObject
{
    /// <summary>
    /// Returns the inbox for the smart rollups.
    /// </summary>
    public RpcObject Inbox => new(this, "inbox/");
        
    internal SmartRollupsAllQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

}