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
        public void Kukai()
        {
            //The same for Temple
            const string mnemonic = "find load relief loop surround tired document coin seven filter draft place music jewel match shoe hope duty thumb cereal lyrics embody talent lumber";
            const string mnemonic1 =
                "guide,dove,reform,mesh,grant,gold,process,fiscal,popular,odor,decorate,bright,  clock,human,  enough,inquiry,upper,hire,foster,law,canal,jeans,pact,push";
            const string address = "tz1dP3E6Pa7Yp8wTeN2CPKfDr5ueLQwiDDTy";
            const string secondAddress = "tz1NWFkgmi2aQRkkjcs1yVY18U1xRrPyWiWA";

            var key = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic)).Derive(HDPath.Parse("m/44'/1729'/0'/0'"));
            var key1 = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic1)).Derive(HDPath.Parse("m/44'/1729'/0'/0'"));
            Assert.Equal(address, key.PubKey.Address);
            Assert.Equal("tz1R8HydaqYsfBE6NhzqAddpYepEuhSh6WBB", key1.PubKey.Address);

            var secondKey = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic)).Derive(HDPath.Parse("m/44'/1729'/1'/0'"));
            var secondKey1 = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic1)).Derive(HDPath.Parse("m/44'/1729'/1'/0'"));
            Assert.Equal(secondAddress, secondKey.PubKey.Address);
            Assert.Equal("tz1h2PMMKPJq2hg55RJ7XDvR3bBFLEhEq1Vx", secondKey1.PubKey.Address);
        }
        
        [Fact]
        public void Atomex()
        {
            //The same for Temple
            const string mnemonic = "blossom kite abuse predict remember acquire useful rifle situate polar noodle retreat surround turn exotic human push depart uncover nut wise snow pulp filter";
            
            const string address = "tz1VmJrHivDEppXkyNfmVm9L1iAskMNP9x9U";
            const string secondAddress = "tz1MYSt3kAzNmvRK3Xo32hUwBzQMpRDGFsPH";
            const string thirdAddress = "tz1VAbTH5i9xzqd6yy1xfvrYcgp9EcQScxAn";

            const string firstPriv = "edsk3QsgTz7AcEsmY8HZS4D164thNvttMvLMbcgvWSXCv4Ee6z51By";
            const string secondPriv = "edsk4AfFtwD79KM1Uxcsop41TrMYWFGvKEM8zKbYSrYMQLFT9uPqS7";
            const string thirdPriv = "edsk3C4ZFwWj92JmzFUQMoSirVCM15jdrfQ6ZbjFxHMzRK7Z4Cw8wa";
            

            var key = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic)).Derive(HDPath.Parse("m/44'/1729'/0'/0'"));
            Assert.Equal(address, key.PubKey.Address);
            //TODO HDKey Key should return proper value
            Assert.Equal(firstPriv, Base58.Convert(key.GetBytes().GetBytes(0, 32), new byte[]{ 13, 15, 58, 7 }));

            var secondKey = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic)).Derive(HDPath.Parse("m/44'/1729'/1'/0'"));
            Assert.Equal(secondAddress, secondKey.PubKey.Address);
            Assert.Equal(secondPriv, Base58.Convert(secondKey.GetBytes().GetBytes(0, 32), new byte[]{ 13, 15, 58, 7 }));

            var thirdKey = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic)).Derive(HDPath.Parse("m/44'/1729'/2'/0'"));
            Assert.Equal(thirdAddress, thirdKey.PubKey.Address);
            Assert.Equal(thirdPriv, Base58.Convert(thirdKey.GetBytes().GetBytes(0, 32), new byte[]{ 13, 15, 58, 7 }));
        }
    }
}