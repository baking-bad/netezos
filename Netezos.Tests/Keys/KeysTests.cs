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
            var key1 = new Key(ECKind.Ed25519);
            var key2 = new Key(ECKind.NistP256);
            var key3 = new Key(ECKind.Secp256k1);
        }
    }
}
