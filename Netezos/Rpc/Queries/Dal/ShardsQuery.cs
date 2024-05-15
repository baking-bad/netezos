namespace Netezos.Rpc.Queries.Dal;

public class ShardsQuery : RpcObject
{
    /// <summary>
    /// Get the shards assignment for a given level and given delegates.
    /// </summary>
    /// <param name="level">Level of the block</param>
    /// <param name="delegates">A Secp256k1 or an Ed25519 public key hash (Base58Check-encoded)</param>
    public Task<dynamic> GetAsync(int level, string delegates)
        => Client.GetJson($"{Query}?level={level}&delegates={delegates}");

    /// <summary>
    /// Get the shards assignment for a given level and given delegates.
    /// </summary>
    /// <param name="level">Level of the block</param>
    /// <param name="delegates">A Secp256k1 or an Ed25519 public key hash (Base58Check-encoded)</param>
    public Task<T?> GetAsync<T>(int level, string delegates)
        => Client.GetJson<T?>($"{Query}?level={level}&delegates={delegates}");

    internal ShardsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}
