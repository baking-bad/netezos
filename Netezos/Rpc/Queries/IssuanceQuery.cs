namespace Netezos.Rpc.Queries;

/// <summary>
/// Rpc query to access issuance data
/// </summary>
public class IssuanceQuery : RpcQuery
{
    /// <summary>
    /// Returns the current expected maximum yearly issuance rate (in %).
    /// </summary>
    public RpcObject CurrentYearlyRate => new(this, "current_yearly_rate/");
        
    /// <summary>
    /// Returns the current expected maximum yearly issuance rate (exact quotient).
    /// </summary>
    public RpcObject CurrentYearlyRateExact => new(this, "current_yearly_rate_exact/");
        
    /// <summary>
    /// Returns the expected issued tez for the provided block and the next 'consensus_rights_delay' cycles.
    /// </summary>
    public RpcObject ExpectedIssuance => new(this, "expected_issuance/");
        
    /// <summary>
    /// Returns the current expected maximum issuance per minute (in mutez).
    /// </summary>
    public RpcObject IssuancePerMinute => new(this, "issuance_per_minute/");
        
    internal IssuanceQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}