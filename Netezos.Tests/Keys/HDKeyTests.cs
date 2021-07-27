using System;
using Dynamic.Json;
using Netezos.Encoding;
using Netezos.Keys;
using Xunit;
using Xunit.Abstractions;

namespace Netezos.Tests.Keys
{
    public class HDKeyTests
    {
        readonly ITestOutputHelper TestOutputHelper;

        public HDKeyTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public void HDTest()
        {
            var path = new HDPath();
            var a = HDKey.FromMnemonic(
                new Mnemonic("icon salute dinner depend radio announce urge hello danger join long toe ridge clever toast opera spot rib outside explain mixture eyebrow brother share"));
            TestOutputHelper.WriteLine(a.Key.PubKey.Address);


            // var hdKey1 = new HDKey(HDStandardKind.Bip32, ECKind.Secp256k1);
            var hdKey2 = new HDKey(HDStandardKind.Slip10, ECKind.Ed25519);

            // var childKey1 = hdKey1.Derive(0).Derive(1, true).Derive(257);
            var childKey2 = hdKey2.Derive(44).Derive(1, true).Derive(257);
            TestOutputHelper.WriteLine(childKey2.Key.PubKey.Address);
        }
    }
}