using System.Net;
namespace Netezos.Rpc
{
    public class BadRequestException : RpcException
    {
        public BadRequestException(string message) : base(HttpStatusCode.BadRequest, message) { }
    }
}
