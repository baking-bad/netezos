using System.Net;
namespace Netezos.Rpc
{
    public class InternalErrorException : RpcException
    {
        public InternalErrorException(string message) : base(HttpStatusCode.InternalServerError, message) { }
    }
}
