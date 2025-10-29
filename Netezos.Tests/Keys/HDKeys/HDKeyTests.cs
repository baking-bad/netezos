using Dynamic.Json;
using Xunit;
using Netezos.Encoding;
using Netezos.Keys;

namespace Netezos.Tests.Keys
{
    public class HDKeyTests
    {
        static void TestPublicKeyDerivation(string path, string seed, ECKind kind = ECKind.Secp256k1)
        {
            var masterKey = HDKey.FromSeed(Hex.Parse(seed), kind);
            var masterPubKey = masterKey.HDPubKey;

            foreach (var uind in HDPath.Parse(path))
            {
                var childKey = masterKey.Derive((int)(uind & 0x7FFFFFFF), (uind & 0x80000000) != 0);
                var childPubKey = childKey.HDPubKey;

                if ((uind & 0x80000000) == 0)
                {
                    var derivedPubKey = masterPubKey.Derive((int)uind);
                    Assert.Equal(derivedPubKey.PubKey.GetBase58(), childPubKey.PubKey.GetBase58());
                    Assert.Equal(Hex.Convert(derivedPubKey.ChainCode), Hex.Convert(childPubKey.ChainCode));
                }

                masterKey = childKey;
                masterPubKey = childPubKey;
            }
        }
        
        [Fact]
        public void TestHdKeyGenerationSecp()
        {
            var path = new HDPath("m/44/1729/0/0/0");
            var key = new HDKey(ECKind.Secp256k1);
            var anotherKey = new HDKey(ECKind.Secp256k1);
            
            Assert.NotEqual(key.Address, anotherKey.Address);
            
            var derived = key.Derive(path);
            var pubDerived = key.HDPubKey.Derive(path);
            
            Assert.Equal(derived.Address, pubDerived.Address);
        }

        [Fact]
        public void TestHDKeyGenerationNist()
        {
            var path = new HDPath("m/44/1729/0/0/1");
            var key = new HDKey(ECKind.NistP256);
            var derived = key.Derive(path);
            var pubDerived = key.HDPubKey.Derive(path);
            
            Assert.Equal(derived.Address, pubDerived.Address);
        }

        [Fact]
        public void TestHDPathParse()
        {
            Assert.Equal("m", HDPath.Parse("").ToString());
            Assert.Equal("m", HDPath.Parse("m").ToString());
            Assert.Equal("m", HDPath.Parse("/").ToString());
            Assert.Equal("m", HDPath.Parse("m/").ToString());
            Assert.Equal("m/1", HDPath.Parse("1").ToString());
            Assert.Equal("m/1", HDPath.Parse("/1").ToString());
            Assert.Equal("m/1", HDPath.Parse("m/1").ToString());
            Assert.Equal("m/1/2'/3'", HDPath.Parse("m/1/2'/3h").ToString());

            Assert.Throws<ArgumentNullException>(() => HDPath.Parse(null!));
            Assert.Throws<FormatException>(() => HDPath.Parse("m/1//2"));
            Assert.Throws<FormatException>(() => HDPath.Parse("m/m"));
            Assert.Throws<FormatException>(() => HDPath.Parse("m/-1"));
            Assert.Throws<FormatException>(() => HDPath.Parse($"m/{0x80000000u}"));
        }

        [Fact]
        public void TestHDPathTryParse()
        {
            Assert.True(HDPath.TryParse("", out var p));
            Assert.Equal("m", p.ToString());

            Assert.True(HDPath.TryParse("m", out p));
            Assert.Equal("m", p.ToString());

            Assert.True(HDPath.TryParse("/", out p));
            Assert.Equal("m", p.ToString());

            Assert.True(HDPath.TryParse("m/", out p));
            Assert.Equal("m", p.ToString());

            Assert.True(HDPath.TryParse("1", out p));
            Assert.Equal("m/1", p.ToString());

            Assert.True(HDPath.TryParse("/1", out p));
            Assert.Equal("m/1", p.ToString());

            Assert.True(HDPath.TryParse("m/1", out p));
            Assert.Equal("m/1", p.ToString());

            Assert.True(HDPath.TryParse("m/1/2'/3h", out p));
            Assert.Equal("m/1/2'/3'", p.ToString());

            Assert.False(HDPath.TryParse(null, out _));
            Assert.False(HDPath.TryParse("m/1//2", out _));
            Assert.False(HDPath.TryParse("m/m", out _));
            Assert.False(HDPath.TryParse("m/-1", out _));
            Assert.False(HDPath.TryParse($"m/{0x80000000u}", out _));
        }

        [Fact]
        public void TestEd25519()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\ed25519.json"))
            {
                var hdKey = HDKey.FromSeed(Hex.Parse((string)sample.seed))
                    .Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, hdKey.Key.GetHex());
                Assert.Equal(sample.chainCode, Hex.Convert(hdKey.ChainCode));
                Assert.Equal(sample.pubKey, hdKey.PubKey.GetHex());
            }
        }

        [Fact]
        public void TestSecp256k1()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\secp256k1.json"))
            {
                var hdKey = HDKey.FromSeed(Hex.Parse((string)sample.seed), ECKind.Secp256k1)
                    .Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, hdKey.Key.GetHex());
                Assert.Equal(sample.chainCode, Hex.Convert(hdKey.ChainCode));
                Assert.Equal(sample.pubKey, hdKey.PubKey.GetHex());
                
                TestPublicKeyDerivation(sample.path, sample.seed, ECKind.Secp256k1);
            }
        }
        

        [Fact]
        public void TestNistp256()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\nistp256.json"))
            {
                var hdKey = HDKey.FromSeed(Hex.Parse((string)sample.seed), ECKind.NistP256)
                    .Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, hdKey.Key.GetHex());
                Assert.Equal(sample.chainCode, Hex.Convert(hdKey.ChainCode));
                Assert.Equal(sample.pubKey, hdKey.PubKey.GetHex());
                
                TestPublicKeyDerivation(sample.path, sample.seed, ECKind.NistP256);
            }
        }

        [Fact]
        public void TestBls12381()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\bls12381.json"))
            {
                var hdKey = HDKey.FromSeed(Hex.Parse((string)sample.seed), ECKind.Bls12381)
                    .Derive((string)sample.path);

                Assert.Equal(sample.privateKey, hdKey.Key.GetHex());
                Assert.Equal(sample.chainCode, Hex.Convert(hdKey.ChainCode));
                Assert.Equal(sample.pubKey, hdKey.PubKey.GetHex());
            }
        }

        [Fact]
        public void Atomex()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\atomex.json"))
            {
                var hdKey = HDKey.FromMnemonic(Mnemonic.Parse((string)sample.mnemonic))
                    .Derive((string)sample.path);
            
                Assert.Equal(sample.privateKey, hdKey.Key.GetBase58());
                Assert.Equal(sample.address, hdKey.Address);
            }
        }
        
        [Fact]
        public void Kukai()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\kukai.json"))
            {
                var password = string.IsNullOrEmpty(sample.password) ? "" : sample.password;
                var hdKey = HDKey.FromMnemonic(Mnemonic.Parse((string)sample.mnemonic), password)
                    .Derive((string)sample.path);
            
                Assert.Equal(sample.address, hdKey.Address);
            }
        }

        [Fact]
        public void BadMnemonics()
        {
            foreach (var sample in DJson.Read(@"..\..\..\Keys\HDKeys\Samples\bad_mnemonics.json"))
            {
                Assert.ThrowsAny<Exception>(() => Mnemonic.Parse((string)sample.mnemonic));
                
                var words = ((string)sample.mnemonic).Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Assert.ThrowsAny<Exception>(() => new Mnemonic(words));
            }
        }
    }
}