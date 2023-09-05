namespace Netezos.Rpc.Queries;

/// <summary>
/// Rpc query to access contract data
/// </summary>
public class SmartRollupQuery : RpcObject
{
    /// <summary>
    /// Genesis information (level and commitment hash) for a smart rollup.
    /// </summary>
    public RpcObject GenesisInfo => new(this, "genesis_info/");
        
    /// <summary>
    /// Initial PVM state hash of smart rollup.
    /// </summary>
    public RpcObject InitialPvmStateHash => new(this, "initial_pvm_state_hash/");
        
    /// <summary>
    /// Kind of smart rollup.
    /// </summary>
    public RpcObject Kind => new(this, "kind/");
        
    /// Level and hash of the last cemented commitment for a smart rollup.
    /// </summary>
    public RpcObject LastCementedCommitmentHashWithLevel => new(this, "last_cemented_commitment_hash_with_level/");
        
    /// List of active stakers' public key hashes of a rollup.
    /// </summary>
    public RpcObject Stakers => new(this, "stakers/");
        
    /// <summary>
        
    internal SmartRollupQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}