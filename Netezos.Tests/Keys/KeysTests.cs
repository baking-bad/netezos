using System;
using System.Threading.Tasks;
using Netezos.Keys;
using Netezos.Rpc;
using Xunit;

namespace Netezos.Tests.Keys
{
    public class KeysTests
    {
        [Fact]
        public void TestKey()
        {
            var key = new Key();

            Assert.NotNull(key);
        }
    }
}
