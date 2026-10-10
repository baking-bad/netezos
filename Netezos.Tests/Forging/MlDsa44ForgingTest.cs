using Dynamic.Json;
using Xunit;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Forging.Models;

namespace Netezos.Tests.Forging
{
    public class MlDsa44ForgingTest
    {
        [Fact]
        public void TestForgeTz5AddressAndMdpkKey()
        {
            foreach (var sample in DJson.Read(@"../../../Keys/Samples/mldsa44.json"))
            {
                var pub = (string)sample.pub;
                var pkh = (string)sample.pkh;

                var pubBytes = Base58.Parse(pub, Prefixes.mdpk);
                var pkhBytes = Base58.Parse(pkh, Prefixes.tz5);
                Assert.Equal(1312, pubBytes.Length);
                Assert.Equal(20, pkhBytes.Length);

                var address = LocalForge.ForgeAddress(pkh);
                Assert.Equal(22, address.Length);
                Assert.Equal(new byte[] { 0, 4 }, address[..2]);
                Assert.Equal(pkhBytes, address[2..]);

                var tzAddress = LocalForge.ForgeTzAddress(pkh);
                Assert.Equal(21, tzAddress.Length);
                Assert.Equal(4, tzAddress[0]);
                Assert.Equal(pkhBytes, tzAddress[1..]);

                var publicKey = LocalForge.ForgePublicKey(pub);
                Assert.Equal(1313, publicKey.Length);
                Assert.Equal(4, publicKey[0]);
                Assert.Equal(pubBytes, publicKey[1..]);

                var bytes = new byte[address.Length + tzAddress.Length + publicKey.Length];
                address.CopyTo(bytes, 0);
                tzAddress.CopyTo(bytes, address.Length);
                publicKey.CopyTo(bytes, address.Length + tzAddress.Length);

                using var reader = new ForgedReader(bytes);
                Assert.Equal(pkh, reader.ReadAddress());
                Assert.Equal(pkh, reader.ReadTzAddress());
                Assert.Equal(pub, reader.ReadPublicKey());
            }
        }

        [Fact]
        public async Task TestForgeAndUnforgeOperationsWithTz5()
        {
            var forge = new LocalForge();
            const string branch = "BKiUgGJ1WitXWoeYFuzK1mCJhFAUg1AnuSAmLzWfKpMc9xWs7u9";

            foreach (var sample in DJson.Read(@"../../../Keys/Samples/mldsa44.json"))
            {
                var pub = (string)sample.pub;
                var pkh = (string)sample.pkh;

                var reveal = new RevealContent
                {
                    Source = pkh,
                    Fee = 1000,
                    Counter = 1,
                    GasLimit = 5000,
                    StorageLimit = 0,
                    PublicKey = pub
                };
                var transaction = new TransactionContent
                {
                    Source = pkh,
                    Fee = 1000,
                    Counter = 2,
                    GasLimit = 5000,
                    StorageLimit = 0,
                    Amount = 1,
                    Destination = pkh
                };

                var bytes = await forge.ForgeOperationGroupAsync(branch, [reveal, transaction]);
                var (unforgedBranch, contents) = await forge.UnforgeOperationAsync(bytes);

                Assert.Equal(branch, unforgedBranch);
                Assert.Equal(2, contents.Count());

                var unforgedReveal = Assert.IsType<RevealContent>(contents.ElementAt(0));
                Assert.Equal(pkh, unforgedReveal.Source);
                Assert.Equal(pub, unforgedReveal.PublicKey);

                var unforgedTransaction = Assert.IsType<TransactionContent>(contents.ElementAt(1));
                Assert.Equal(pkh, unforgedTransaction.Source);
                Assert.Equal(pkh, unforgedTransaction.Destination);
            }
        }
    }
}
