using Dynamic.Json;
using System;
using System.Threading.Tasks;
using Netezos.Keys;
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

            var hdKey1 = new HDKey(HDStandardKind.Bip32, ECKind.Secp256k1);
            var hdKey2 = new HDKey(HDStandardKind.Slip10, ECKind.Ed25519);

            var childKey1 = hdKey1.Derive(0).Derive(1, true).Derive(257);
            var childKey2 = hdKey2.Derive(0).Derive(1, true).Derive(257);
        }

        [Fact]
        public void TestBIP32Key()
        {
        }

        [Fact]
        public void TestSLIP10Key()
        {
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
