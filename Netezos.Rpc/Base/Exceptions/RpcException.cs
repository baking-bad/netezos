using System;
using System.Net;

namespace Netezos.Rpc
{
    public class RpcException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public RpcException(HttpStatusCode code, string message) : base(message) => StatusCode = code;
    }
}
