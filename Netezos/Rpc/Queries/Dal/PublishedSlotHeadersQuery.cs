namespace Netezos.Rpc.Queries.Dal;

public class PublishedSlotHeadersQuery : RpcObject
{
    /// <summary>
    /// Get the published slots headers for the current level.
    /// </summary>
    /// <returns></returns>
    public new Task<dynamic> GetAsync() => Client.GetJson(Query);

    /// <summary>
    /// Get the published slots headers for the given level
    /// </summary>
    /// <param name="level">Level of the block</param>
    /// <returns></returns>
    public Task<dynamic> GetAsync(int level) => Client.GetJson($"{Query}?level={level}");

    internal PublishedSlotHeadersQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
}
