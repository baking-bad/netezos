namespace Netezos.Rpc.Queries.Post;

/// <summary>
/// Rpc query to access the contract's balance of ticket with specified ticketer, content type, and content.
/// </summary>
public class TicketBalanceQuery : RpcMethod
{
    internal TicketBalanceQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

    /// <summary>
    /// Gets the contract's balance of ticket with specified ticketer, content type, and content.
    /// </summary>
    /// <param name="ticketer">Ticketer contract</param>
    /// <param name="contentType">Ticket content type</param>
    /// <param name="content">Ticket content</param>
    /// <returns></returns>
    public Task<dynamic> PostAsync(string ticketer, object contentType, object content)
        => PostAsync(new
        {
            ticketer,
            content_type = contentType,
            content
        });
}