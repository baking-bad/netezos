using Xunit;
using Netezos.Contracts;
using Netezos.Encoding;

namespace Netezos.Tests.Contracts
{
    public class TestSignatureSchema
    {
        static readonly SignatureSchema Schema = new(new MichelinePrim { Prim = PrimType.signature });

        [Theory]
        [InlineData("sigQFenAPMsrMxVvgH1K33sJgj5VqD3gajK1sBJyEugCzZ9EgTvHGiXii9opAgkei7tY1qpJKyB37YGdGGMAWdgodPyDcQvg", 64)]
        [InlineData("BLsigBftWraYBgd65KzPAtgpy77UDvc41vSHYTp3c4E12Watjhq2UG75ohAsi4pAAx9pPDc2wCtwALoUG3WBuWDvDRvr62EFendJtb3cxZqNa2DQJ9oHPufYxHJWmKgx1DhsbbWfnjBz3o", 96)]
        public void TestFlattenOptimized(string signature, int length)
        {
            var optimized = Assert.IsType<MichelineBytes>(Schema.Optimize(new MichelineString(signature)));
            Assert.Equal(length, optimized.Value.Length);
            Assert.Equal(signature, Schema.Flatten(optimized));
            Assert.Equal($"\"{signature}\"", Schema.Humanize(optimized));
        }

        [Fact]
        public void TestFlattenOptimizedMlDsa44()
        {
            // synthetic signature, since ML-DSA-44 signing is not implemented yet (the schema only looks at the length)
            var bytes = Enumerable.Range(0, 2420).Select(i => (byte)i).ToArray();
            var signature = Base58.Convert(bytes, Prefixes.mdsig);

            var optimized = Assert.IsType<MichelineBytes>(Schema.Optimize(new MichelineString(signature)));
            Assert.Equal(bytes, optimized.Value);
            Assert.Equal(signature, Schema.Flatten(optimized));
            Assert.Equal($"\"{signature}\"", Schema.Humanize(optimized));
        }

        [Fact]
        public void TestFlattenOptimizedEd25519()
        {
            // the protocol prints any 64-byte signature in the generic form
            const string edsig = "edsigtjJQ2wppYpkR16kk4QU6mH758tUoSahfPcykQxpVaU1wJ5HfBy1tANeu9BL7kWYdrGUfqWubYtiU6HNNrV8mtUWNScBWAu";
            var bytes = Base58.Parse(edsig, Prefixes.edsig);

            var optimized = Assert.IsType<MichelineBytes>(Schema.Optimize(new MichelineString(edsig)));
            Assert.Equal(bytes, optimized.Value);
            Assert.Equal(Base58.Convert(bytes, Prefixes.sig), Schema.Flatten(optimized));
        }

        [Theory]
        [InlineData(63)]
        [InlineData(97)]
        [InlineData(2421)]
        public void TestFlattenUnknownLength(int length)
        {
            var bytes = Enumerable.Range(0, length).Select(i => (byte)i).ToArray();
            Assert.Equal(Hex.Convert(bytes), Schema.Flatten(new MichelineBytes(bytes)));
        }
    }
}
