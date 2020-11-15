using Dynamic.Json;
using Xunit;

using Netezos.Encoding;
using Netezos.Keys;

namespace Netezos.Tests.Keys
{
    public class KeysTests
    {
        [Fact]
        public void TestEd25519()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\Samples\ed25519.json"))
            {
                var key = Key.FromBase58(sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
            }
        }

        [Fact]
        public void TestSecp256k1()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\Samples\secp256k1.json"))
            {
                var key = Key.FromBase58(sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
            }
        }

        [Fact]
        public void TestNistp256()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\Samples\nistp256.json"))
            {
                var key = Key.FromBase58(sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
            }
        }
    }
}
