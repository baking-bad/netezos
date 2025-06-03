using System.Net;

namespace Netezos.Rpc
{
    /// <summary>
    /// Represents the RPC error with HTTP status code 500
    /// </summary>
    public class InternalErrorException(string message) : RpcException(HttpStatusCode.InternalServerError, message) { }
}
