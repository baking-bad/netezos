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
            foreach (var sample in DJson.Read(@"../../../Keys/Samples/ed25519.json"))
            {
                var key = Key.FromBase58((string)sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
                Assert.True(key.PubKey.Verify(data, sample.sig));
            }

            var testKey = new Key(ECKind.Ed25519);
            const string msg = "TestMessage";
            var sign = testKey.Sign(msg);
            Assert.True(testKey.PubKey.Verify(msg, sign));
            Assert.False(testKey.PubKey.Verify(msg, "wrongSignature"));
            Assert.False(testKey.PubKey.Verify(msg, null!));
            Assert.False(testKey.PubKey.Verify("wrongMessage", sign));
            Assert.False(testKey.PubKey.Verify((string)null!, sign));
        }

        [Fact]
        public void TestSecp256k1()
        {
            foreach (var sample in DJson.Read(@"../../../Keys/Samples/secp256k1.json"))
            {
                var key = Key.FromBase58(sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
                Assert.True(key.PubKey.Verify(data, sample.sig));
            }
            
            var testKey = new Key(ECKind.Secp256k1);
            const string msg = "TestMessage";
            var sign = testKey.Sign(msg);
            Assert.True(testKey.PubKey.Verify(msg, sign));
            Assert.False(testKey.PubKey.Verify(msg, "wrongSignature"));
            Assert.False(testKey.PubKey.Verify(msg, null!));
            Assert.False(testKey.PubKey.Verify("wrongMessage", sign));
            Assert.False(testKey.PubKey.Verify((string)null!, sign));
        }

        [Fact]
        public void TestNistp256()
        {
            foreach (var sample in DJson.Read(@"../../../Keys/Samples/nistp256.json"))
            {
                var key = Key.FromBase58(sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
                Assert.True(key.PubKey.Verify(data, sample.sig));
            }
            
            var testKey = new Key(ECKind.NistP256);
            const string msg = "TestMessage";
            var sign = testKey.Sign(msg);
            Assert.True(testKey.PubKey.Verify(msg, sign));
            Assert.False(testKey.PubKey.Verify(msg, "wrongSignature"));
            Assert.False(testKey.PubKey.Verify(msg, null!));
            Assert.False(testKey.PubKey.Verify("wrongMessage", sign));
            Assert.False(testKey.PubKey.Verify((string)null!, sign));
        }

        [Fact]
        public void TestBls12381()
        {
            foreach (var sample in DJson.Read(@"../../../Keys/Samples/bls12381.json"))
            {
                var key = Key.FromBase58((string)sample.prv);
                var data = Hex.Parse(sample.data);

                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);
                Assert.Equal(sample.sig, key.Sign(data));
                Assert.True(key.PubKey.Verify(data, sample.sig));
            }

            var testKey = new Key(ECKind.Bls12381);
            const string msg = "TestMessage";
            var sign = testKey.Sign(msg);
            Assert.True(testKey.PubKey.Verify(msg, sign));
            Assert.False(testKey.PubKey.Verify(msg, "wrongSignature"));
            Assert.False(testKey.PubKey.Verify(msg, null!));
            Assert.False(testKey.PubKey.Verify("wrongMessage", sign));
            Assert.False(testKey.PubKey.Verify((string)null!, sign));
        }

        [Fact]
        public void TestMlDsa44()
        {
            foreach (var sample in DJson.Read(@"../../../Keys/Samples/mldsa44.json"))
            {
                var key = Key.FromBase58((string)sample.prv);

                Assert.Equal(sample.prv, key.GetBase58());
                Assert.Equal(sample.pub, key.PubKey.GetBase58());
                Assert.Equal(sample.pkh, key.PubKey.Address);

                var pubKey = PubKey.FromBase58((string)sample.pub);

                Assert.Equal(sample.pub, pubKey.GetBase58());
                Assert.Equal(sample.pkh, pubKey.Address);
                Assert.Equal(key.PubKey.GetBytes(), pubKey.GetBytes());
                Assert.Equal(sample.pub, PubKey.FromBytes(pubKey.GetBytes(), ECKind.MlDsa44).GetBase58());
                Assert.Equal(sample.pub, PubKey.FromHex(pubKey.GetHex(), ECKind.MlDsa44).GetBase58());

                // only encoding is supported so far
                var signature = Base58.Convert(new byte[2420], Prefixes.mdsig);
                Assert.Throws<NotImplementedException>(() => key.Sign("TestMessage"));
                Assert.Throws<NotImplementedException>(() => key.GetProofOfPossession());
                Assert.Throws<NotImplementedException>(() => pubKey.Verify("TestMessage", signature));
            }

            Assert.Throws<NotImplementedException>(() => new Key(ECKind.MlDsa44));
            Assert.Throws<ArgumentException>(() => PubKey.FromBytes(new byte[48], ECKind.MlDsa44));
            Assert.Throws<ArgumentException>(() => Key.FromBytes(new byte[32], ECKind.MlDsa44));
        }
    }
}
