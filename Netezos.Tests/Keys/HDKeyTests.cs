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
        
        private const string Ed25519Vector1KeyHexExpected = "2b4be7f19ee27bbf30c667b642d5f4aa69fd169872f8fc3059c08ebae2eb19e7";
        private const string Ed25519Vector1ChainCodeExpected = "90046a93de5380a72b5e45010748567d5ea02bbf6522f979e05c0d8d8ca9fffb";   
        
        private const string Secp256k1Vector1KeyHexExpected = "e8f32e723decf4051aefac8e2c93c9c5b214313817cdb01a1494b917c8436b35";
        private const string Secp256k1Vector1ChainCodeExpected = "873dff81c02f525623fd1fe5167eac3a55a049de3d314bb42ee227ffed37d508";

        private const string Vector2Seed = "fffcf9f6f3f0edeae7e4e1dedbd8d5d2cfccc9c6c3c0bdbab7b4b1aeaba8a5a29f9c999693908d8a8784817e7b7875726f6c696663605d5a5754514e4b484542";
        
        private const string Ed25519Vector2KeyHexExpected = "171cb88b1b3c1db25add599712e36245d75bc65a1a5c9e18d76f9f2b1eab4012";
        private const string Ed25519Vector2ChainCodeExpected = "ef70a74db9c3a5af931b5fe73ed8e1a53464133654fd55e7a66f8570b8e33c3b";

        
        readonly uint hardenedOffset = 0x80000000;

        #region helpers

        private (byte[] Key, byte[] ChainCode) TestMasterKeyFromSeed(string seed, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var key = HDKey.FromHex(seed, hdStandard, ecKind);


            var masterKeyFromSeed = key.Key.GetBytes();


            return (masterKeyFromSeed.GetBytes(0, 32), masterKeyFromSeed.GetBytes(32, masterKeyFromSeed.Length - 32));
        }

        private (byte[] Key, byte[] ChainCode) TestDerivePath(string path, string seed,HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var key = HDKey.FromHex(seed, hdStandard, ecKind);

            var derivePath = key.Derive(HDPath.Parse(path)).Key.GetBytes();
            
            var c = Hex.Convert(derivePath);

            return (derivePath.GetBytes(0, 32), derivePath.GetBytes(32, 32));
        }

        private byte[] TestGetPublicKey(string path, string seed,HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var key = HDKey.FromHex(seed, hdStandard, ecKind);

            var derivePath = key.Derive(HDPath.Parse(path)).Key;

            return derivePath.PubKey.GetBytes();
        }

        private byte[] TestGetPublicKey(byte[] privateKey, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var key = new HDKey(privateKey, hdStandard, ecKind);

            var publicKey = key.PubKey.GetBytes();

            return publicKey;
        }

        #endregion

        [Fact]
        public void TestHDPath()
        {
            var keyPath = new HDPath(new uint[] { 0x8000002Cu, 1u });
            var a = keyPath.ToString();
            Assert.Equal("44'/1", keyPath.ToString());
        }

        #region Ed25519

        [Fact]
        public void TestVector1Ed25519Test1()
        {
            const string expectedPath = "m/0'";
            const string expectedChainCode = "8b59aa11380b624e81507a27fedda59fea6d0b779a778918a2fd3590e16e9c69";
            const string expectedKey = "68e0fe46dfb67e368c75379acec591dad19df3cde26e63b93a8e704f1dade7a3";
            const string expectedPublicKey = "008c8a13df77a28f3445213a0f432fde644acaa215fc72dcdf300d5efaa85d350c";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed);

            Assert.Equal(Ed25519Vector1KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Ed25519Vector1ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));


            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestVector1Ed25519Test2()
        {
            const string expectedPath = "m/0'/1'";
            const string expectedChainCode = "a320425f77d1b5c2505a6b1b27382b37368ee640e3557c315416801243552f14";
            const string expectedKey = "b1d0bad404bf35da785a64ca1ac54b2617211d2777696fbffaf208f746ae84f2";
            const string expectedPublicKey = "001932a5270f335bed617d5b935c80aedb1a35bd9fc1e31acafd5372c30f5c1187";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed);
            Assert.Equal(Ed25519Vector1KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector1ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector1Ed25519Test3()
        {
            const string expectedPath = "m/0'/1'/2'";
            const string expectedChainCode = "2e69929e00b5ab250f49c3fb1c12f252de4fed2c1db88387094a0f8c4c9ccd6c";
            const string expectedKey = "92a5b23c0b8a99e37d07df3fb9966917f5d06e02ddbd909c7e184371463e9fc9";
            const string expectedPublicKey = "00ae98736566d30ed0e9d2f4486a64bc95740d89c7db33f52121f8ea8f76ff0fc1";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed);
            Assert.Equal(Ed25519Vector1KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector1ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector1Ed25519Test4()
        {
            const string expectedPath = "m/0'/1'/2'/2'";
            const string expectedChainCode = "8f6d87f93d750e0efccda017d662a1b31a266e4a6f5993b15f5c1f07f74dd5cc";
            const string expectedKey = "30d1dc7e5fc04c31219ab25a27ae00b50f6fd66622f6e9c913253d6511d1e662";
            const string expectedPublicKey = "008abae2d66361c879b900d204ad2cc4984fa2aa344dd7ddc46007329ac76c429c";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed);
            Assert.Equal(Ed25519Vector1KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector1ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector1Ed25519Test5()
        {
            const string expectedPath = "m/0'/1'/2'/2'/1000000000'";
            const string expectedChainCode = "68789923a0cac2cd5a29172a475fe9e0fb14cd6adb5ad98a3fa70333e7afa230";
            const string expectedKey = "8f94d394a8e8fd6b1bc2f3f49f5c47e385281d5c17e65324b0f62483e37e8793";
            const string expectedPublicKey = "003c24da049451555d51a7014a37337aa4e12d41e485abccfa46b47dfb2af54b7a";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed);
            Assert.Equal(Ed25519Vector1KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector1ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector2Ed25519Test1()
        {
            const string expectedPath = "m/0'";
            const string expectedChainCode = "0b78a3226f915c082bf118f83618a618ab6dec793752624cbeb622acb562862d";
            const string expectedKey = "1559eb2bbec5790b0c65d8693e4d0875b1747f4970ae8b650486ed7470845635";
            const string expectedPublicKey = "0086fab68dcb57aa196c77c5f264f215a112c22a912c10d123b0d03c3c28ef1037";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed);
            Assert.Equal(Ed25519Vector2KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector2ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector2Ed25519Test2()
        {
            const string expectedPath = "m/0'/2147483647'";
            const string expectedChainCode = "138f0b2551bcafeca6ff2aa88ba8ed0ed8de070841f0c4ef0165df8181eaad7f";
            const string expectedKey = "ea4f5bfe8694d8bb74b7b59404632fd5968b774ed545e810de9c32a4fb4192f4";
            const string expectedPublicKey = "005ba3b9ac6e90e83effcd25ac4e58a1365a9e35a3d3ae5eb07b9e4d90bcf7506d";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed);
            Assert.Equal(Ed25519Vector2KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector2ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector2Ed25519Test3()
        {
            const string expectedPath = "m/0'/2147483647'/1'";
            const string expectedChainCode = "73bd9fff1cfbde33a1b846c27085f711c0fe2d66fd32e139d3ebc28e5a4a6b90";
            const string expectedKey = "3757c7577170179c7868353ada796c839135b3d30554bbb74a4b1e4a5a58505c";
            const string expectedPublicKey = "002e66aa57069c86cc18249aecf5cb5a9cebbfd6fadeab056254763874a9352b45";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed);
            Assert.Equal(Ed25519Vector2KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector2ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        [Fact]
        public void TestVector2Ed25519Test4()
        {
            const string expectedPath = "m/0'/2147483647'/1'/2147483646'";
            const string expectedChainCode = "0902fe8a29f9140480a00ef244bd183e8a13288e4412d8389d140aac1794825a";
            const string expectedKey = "5837736c89570de861ebc173b1086da4f505d4adb387c6a1b1342d5e4ac9ec72";
            const string expectedPublicKey = "00e33c0f7d81d843c572275f287498e8d408654fdf0d1e065b84e2e6f157aab09b";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed);
            Assert.Equal(Ed25519Vector2KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector2ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }


        [Fact]
        public void TestVector2Ed25519Test5()
        {
            const string expectedPath = "m/0'/2147483647'/1'/2147483646'/2'";
            const string expectedChainCode = "5d70af781f3a37b829f0d060924d5e960bdc02e85423494afc0b1a41bbe196d4";
            const string expectedKey = "551d333177df541ad876a60ea71f00447931c0a9da16f227c11ea080d7391b8d";
            const string expectedPublicKey = "0047150c75db263559a70d5778bf36abbab30fb061ad69f69ece61a72b0cfa4fc0";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed);
            Assert.Equal(Ed25519Vector2KeyHexExpected, testMasterKeyFromSeed.Key.ToStringHex());
            Assert.Equal(Ed25519Vector2ChainCodeExpected, testMasterKeyFromSeed.ChainCode.ToStringHex());

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed);
            Assert.Equal(expectedKey, testDerivePath.Key.ToStringHex());
            Assert.Equal(expectedChainCode, testDerivePath.ChainCode.ToStringHex());

            var testPublicKey = TestGetPublicKey(testDerivePath.Key);
            Assert.Equal(expectedPublicKey, testPublicKey.ToStringHex());
        }

        #endregion

        #region secp256k1

        [Fact]
        public void TestSecp256k1Vector1Test1()
        {
            const string expectedPath = "m/0'";
            const string expectedChainCode = "47fdacbd0f1097043b78c63c20c34ef4ed9a111d980047ad16282c7ae6236141";
            const string expectedKey = "edb2e14f9ee77d26dd93b4ecede8d16ed408ce149b6cd80b0715a2d911a0afea";
            const string expectedPublicKey = "035a784662a4a20a65bf6aab9ae98a6c068a81c52e4b032c0fb5400c706cfccc56";
            var tst =
                "04bfb2dd60fa8921c2a4085ec15507a921f49cdc839f27f0f280e9c1495d44b547fdacbd0f1097043b78c63c20c34ef4ed9a111d980047ad16282c7ae6236141";
            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            
            Assert.Equal(Secp256k1Vector1KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector1ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestSecp256k1Vector1Test2()
        {
            const string expectedPath = "m/0'/1";
            const string expectedChainCode = "2a7857631386ba23dacac34180dd1983734e444fdbf774041578e9b6adb37c19";
            const string expectedKey = "3c6cb8d0f6a264c91ea8b5030fadaa8e538b020f0a387421a12de9319dc93368";
            const string expectedPublicKey = "03501e454bf00751f24b1b489aa925215d66af2234e3891c3b21a52bedb3cd711c";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector1KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector1ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestSecp256k1Vector1Test3()
        {
            //TODO TBD
            const string expectedPath = "m/0'/1/2'";
            const string expectedChainCode = "04466b9cc8e161e966409ca52986c584f07e9dc81f735db683c3ff6ec7b1503f";
            const string expectedKey = "cbce0d719ecf7431d88e6a89fa1483e02e35092af60c042b1df2ff59fa424dca";
            const string expectedPublicKey = "0357bfe1e341d01c69fe5654309956cbea516822fba8a601743a012a7896ee8dc2";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector1KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector1ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void Kukai()
        {
            //TODO Working without zeroing byte GetChildPublicKey()
            //The same for Temple
            var mnemonic =
                "find load relief loop surround tired document coin seven filter draft place music jewel match shoe hope duty thumb cereal lyrics embody talent lumber";
            var address = "tz1dP3E6Pa7Yp8wTeN2CPKfDr5ueLQwiDDTy";
            var secondAddress = "tz1NWFkgmi2aQRkkjcs1yVY18U1xRrPyWiWA";

            var key = HDKey.FromMnemonic(Mnemonic.Parse(mnemonic)).Derive(HDPath.Parse("m/44'/1729'/0'/0'"));
            Assert.Equal(address, key.PubKey.Address);

        }

        #endregion
        //TODO TestVector3
        //TODO TestVector4
    }
}