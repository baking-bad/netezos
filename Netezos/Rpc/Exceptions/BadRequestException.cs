using System.Net;

namespace Netezos.Rpc
{
    /// <summary>
    /// Represents the RPC error with HTTP status code 400
    /// </summary>
    public class BadRequestException : RpcException
    {
        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) { }
    }
}
