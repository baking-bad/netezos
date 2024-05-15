namespace Netezos.Rpc.Queries.Dal;

public class PublishedSlotHeadersQuery : RpcObject
{
    /// <summary>
    /// Get the published slots headers for the given level
    /// </summary>
    /// <param name="level">Level of the block</param>
    /// <returns></returns>
    public Task<dynamic> GetAsync(int level) => Client.GetJson($"{Query}?level={level}");

    /// <summary>
    /// Get the published slots headers for the given level
    /// </summary>
    /// <param name="level">Level of the block</param>
    /// <returns></returns>
    public Task<T?> GetAsync<T>(int level) => Client.GetJson<T?>($"{Query}?level={level}");

    internal PublishedSlotHeadersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}
