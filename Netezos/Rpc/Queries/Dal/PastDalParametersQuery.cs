namespace Netezos.Rpc.Queries.Dal;

/// <summary>
/// Rpc query to access DAL past parameters by block level
/// </summary>
public class PastDalParametersQuery : RpcObject
{
    /// <summary>
    /// Gets the query to the DAL parameters used at the given block level
    /// </summary>
    /// <param name="blockLevel">Block level</param>
    public RpcObject this[int blockLevel] => new(this, $"{blockLevel}/");

    internal PastDalParametersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}