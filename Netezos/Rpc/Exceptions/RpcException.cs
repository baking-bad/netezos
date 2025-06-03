using System.Net;

namespace Netezos.Rpc
{
    /// <summary>
    /// Represents errors that occur during RPC requests execution
    /// </summary>
    public class RpcException(HttpStatusCode code, string message) : Exception(message)
    {
        /// <summary>
        /// Gets HTTP status code returned by the RPC server
        /// </summary>
        public HttpStatusCode StatusCode { get; set; } = code;
    }
}
