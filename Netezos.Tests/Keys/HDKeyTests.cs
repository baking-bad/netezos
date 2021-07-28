using System;
using Dynamic.Json;
using Netezos.Encoding;
using Netezos.Keys;
using Xunit;
using Xunit.Abstractions;
using Netezos;

namespace Netezos.Tests.Keys
{
    public class HDKeyTests
    {
        
        private const string Vector1Seed = "000102030405060708090a0b0c0d0e0f";
        private const string Vector1KeyHexExpected = "2b4be7f19ee27bbf30c667b642d5f4aa69fd169872f8fc3059c08ebae2eb19e7";
        private const string Vector1ChainCodeExpected = "90046a93de5380a72b5e45010748567d5ea02bbf6522f979e05c0d8d8ca9fffb";

        private const string Vector2Seed = "fffcf9f6f3f0edeae7e4e1dedbd8d5d2cfccc9c6c3c0bdbab7b4b1aeaba8a5a29f9c999693908d8a8784817e7b7875726f6c696663605d5a5754514e4b484542";
        private const string Vector2KeyHexExpected = "171cb88b1b3c1db25add599712e36245d75bc65a1a5c9e18d76f9f2b1eab4012";
        private const string Vector2ChainCodeExpected = "ef70a74db9c3a5af931b5fe73ed8e1a53464133654fd55e7a66f8570b8e33c3b";
        
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

            // var childKey1 = hdKey1.Derive(0).Derive(1, true).Derive(257);
            var childKey2 = a.Derive(44).Derive(1729).Derive(0).Derive(0).Derive(0);
            var b = a.Derive(new HDPath("m/44'/1729'/0'/0'/0'"));
            TestOutputHelper.WriteLine(childKey2.Key.PubKey.Address);
            TestOutputHelper.WriteLine(b.Key.PubKey.Address);
        }
        
        private (byte[] Key, byte[] ChainCode) TestMasterKeyFromSeed(string seed)
        {
            var key = HDKey.FromHex(seed, HDStandardKind.Slip10, ECKind.Ed25519);

            
            var masterKeyFromSeed = key.Key.GetBytes();

            
            return (masterKeyFromSeed.GetBytes(0, 32), masterKeyFromSeed.GetBytes(32, masterKeyFromSeed.Length - 32));
        }
        
        [Fact]
        public void TestVector1_Test1()
        {
            const string expectedPath = "m/0'";
            const string expectedChainCode = "8b59aa11380b624e81507a27fedda59fea6d0b779a778918a2fd3590e16e9c69";
            const string expectedKey = "68e0fe46dfb67e368c75379acec591dad19df3cde26e63b93a8e704f1dade7a3";
            const string expectedPublicKey = "008c8a13df77a28f3445213a0f432fde644acaa215fc72dcdf300d5efaa85d350c";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed);

            Assert.Equal(Vector1KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Vector1ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            // var testDerivePath = TestDerivePath(expectedPath, Vector1Seed);
            // Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            // Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());
            //
            // var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            // Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }
    }
}