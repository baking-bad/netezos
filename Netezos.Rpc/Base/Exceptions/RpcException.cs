using System;
using System.Net;

namespace Netezos.Rpc
{
    /// <summary>
    /// Represents errors that occur during RPC requests execution
    /// </summary>
    public class RpcException : Exception
    {
        /// <summary>
        /// Gets HTTP status code returned by the RPC server
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        public RpcException(HttpStatusCode code, string message) : base(message) => StatusCode = code;
    }
}
