using System.Net;

namespace Netezos.Rpc
{
    /// <summary>
    /// Represents the RPC error with HTTP status code 400
    /// </summary>
    public class BadRequestException(string message) : RpcException(HttpStatusCode.BadRequest, message) { }
}
