using Xunit;
using Netezos.Contracts;
using Netezos.Encoding;

namespace Netezos.Tests.Contracts
{
    public class TestTicketSchema
    {
        [Fact]
        public void TestInputIsNotModified()
        {
            var type = (MichelinePrim)Micheline.FromJson("""{"prim":"ticket","args":[{"prim":"string"}]}""")!;
            var json = type.ToJson();

            var schema = new TicketSchema(type);

            Assert.Equal(json, type.ToJson());
            Assert.Equal("content", ((schema.Ticket.Right as PairSchema)!.Left).Name);
        }
    }
}
