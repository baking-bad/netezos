using Dynamic.Json;
using Netezos.Encoding;
using Netezos.Keys;
using Xunit;

namespace Netezos.Tests.Keys
{
    public class HDKeyTests
    {
        static void TestPublicKeyDerivation(string path, string seed, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Secp256k1)
        {
            var key = HDKey.FromHex(seed, hdStandard, ecKind);
            var pubKey = HDPubKey.FromBytes(key.PubKey.GetBytes(), key.ChainCode, hdStandard, ecKind);

            foreach (var index in HDPath.Parse(path).Indexes)
            {
                var keyNew = key.Derive(index);
                var pubKeyNew = keyNew.HdPubKey;

                if ((index & 0x80000000) == 0)
                {
                    var derivedPubKey = pubKey.Derive(index);
                    Assert.Equal(derivedPubKey.GetBase58(), pubKeyNew.GetBase58());
                    Assert.Equal(derivedPubKey.GetChainCodeHex(), pubKeyNew.GetChainCodeHex());
                }

                key = keyNew;
                pubKey = pubKeyNew;
            }
        }
        
        [Fact]
        public void TestHdKeyGenerationSecp()
        {
            var path = new HDPath("m/44/1729/0/0/0");
            var key = new HDKey(HDStandardKind.Slip10, ECKind.Secp256k1);
            var anotherKey = new HDKey(HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.NotEqual(key.PubKey.Address, anotherKey.PubKey.Address);
            var derived = key.Derive(path);
            var pubDerived = key.HdPubKey.Derive(path);
            Assert.Equal(derived.PubKey.Address, pubDerived.Address);
        }

        [Fact]
        public void TestHDKeyGenerationNist()
        {
            var path = new HDPath("m/44/1729/0/0/1");
            var key = new HDKey(HDStandardKind.Slip10, ECKind.NistP256);
            var derived = key.Derive(path);
            var pubDerived = key.HdPubKey.Derive(path);
            Assert.Equal(derived.PubKey.Address, pubDerived.Address);
        }
        
        [Fact]
        public void TestHDPath()
        {
            var keyPath = new HDPath(0x8000002Cu, 1u);
            var a = keyPath.ToString();
            Assert.Equal("44'/1", keyPath.ToString());
        }

        [Fact]
        public void TestEd25519()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\ed25519.json"))
            {
                var key = HDKey.FromHex((string)sample.seed).Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, key.Key.GetHex());
                Assert.Equal(sample.chainCode, key.ChainCode.ToStringHex());
                Assert.Equal(sample.pubKey, key.PubKey.GetHex());
            }
        }

        [Fact]
        public void TestSecp256k1()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\secp256k1.json"))
            {
                var key = HDKey.FromHex((string)sample.seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, key.Key.GetHex());
                Assert.Equal(sample.chainCode, key.ChainCode.ToStringHex());
                Assert.Equal(sample.pubKey, key.PubKey.GetHex());
                
                TestPublicKeyDerivation(sample.path, sample.seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            }
        }
        

        [Fact]
        public void TestNistp256()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\nistp256.json"))
            {
                var key = HDKey.FromHex((string)sample.seed, HDStandardKind.Slip10, ECKind.NistP256).Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, key.Key.GetHex());
                Assert.Equal(sample.chainCode, key.ChainCode.ToStringHex());
                Assert.Equal(sample.pubKey, key.PubKey.GetHex());
                
                TestPublicKeyDerivation(sample.path, sample.seed, HDStandardKind.Slip10, ECKind.NistP256);
            }
        }
        
        [Fact]
        public void Atomex()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\atomex.json"))
            {
                var key = HDKey.FromMnemonic(Mnemonic.Parse((string)sample.mnemonic)).Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, key.Key.GetBase58());
                Assert.Equal(sample.address, key.PubKey.Address);
            }
        }
        
        [Fact]
        public void Kukai()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\kukai.json"))
            {
                var key = HDKey.FromMnemonic(Mnemonic.Parse((string)sample.mnemonic)).Derive((string)sample.path);
            
                Assert.Equal(sample.address, key.PubKey.Address);
            }
        }
    }
}