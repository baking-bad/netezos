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
        
        private const string Secp256k1Vector2KeyHexExpected = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
        private const string Secp256k1Vector2ChainCodeExpected = "60499f801b896d83179a4374aeb7822aaeaceaa0db1f85ee3e904c4defbd9689";

        
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
        public void TestSecp256k1Vector1Test4()
        {
            const string expectedPath = "m/0'/1/2'/2";
            const string expectedChainCode = "cfb71883f01676f587d023cc53a35bc7f88f724b1f8c2892ac1275ac822a3edd";
            const string expectedKey = "0f479245fb19a38a1954c5c7c0ebab2f9bdfd96a17563ef28a6a4b1a2a764ef4";
            const string expectedPublicKey = "02e8445082a72f29b75ca48748a914df60622a609cacfce8ed0e35804560741d29";

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
        public void TestSecp256k1Vector1Test5()
        {
            const string expectedPath = "m/0'/1/2'/2/1000000000";
            const string expectedChainCode = "c783e67b921d2beb8f6b389cc646d7263b4145701dadd2161548a8b078e65e9e";
            const string expectedKey = "471b76e389e528d6de6d816857e012c5455051cad6660850e58372a6c3e6e7c8";
            const string expectedPublicKey = "022a471424da5e657499d1ff51cb43c47481a03b1e77f951fe64cec9f5a48f7011";

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
        public void TestSecp256k1Vector2Test1()
        {
        /*private const string Secp256k1Vector2KeyHexExpected = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
        private const string Secp256k1Vector2ChainCodeExpected = "03cbcaa9c98c877a26977d00825c956a238e8dddfbd322cce4f74b0b5bd6ace4a7";*/
            const string expectedPath = "m/0";
            const string expectedChainCode = "f0909affaa7ee7abe5dd4e100598d4dc53cd709d5a5c2cac40e7412f232f7c9c";
            const string expectedKey = "abe74a98f6c7eabee0428f53798f0ab8aa1bd37873999041703c742f15ac7e1e";
            const string expectedPublicKey = "02fc9e5af0ac8d9b3cecfe2a888e2117ba3d089d8585886c9c826b6b22a98d12ea";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector2KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector2ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestSecp256k1Vector2Test2()
        {
        /*private const string Secp256k1Vector2KeyHexExpected = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
        private const string Secp256k1Vector2ChainCodeExpected = "03cbcaa9c98c877a26977d00825c956a238e8dddfbd322cce4f74b0b5bd6ace4a7";*/
            const string expectedPath = "m/0/2147483647'";
            const string expectedChainCode = "be17a268474a6bb9c61e1d720cf6215e2a88c5406c4aee7b38547f585c9a37d9";
            const string expectedKey = "877c779ad9687164e9c2f4f0f4ff0340814392330693ce95a58fe18fd52e6e93";
            const string expectedPublicKey = "03c01e7425647bdefa82b12d9bad5e3e6865bee0502694b94ca58b666abc0a5c3b";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector2KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector2ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestSecp256k1Vector2Test3()
        {
        /*private const string Secp256k1Vector2KeyHexExpected = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
        private const string Secp256k1Vector2ChainCodeExpected = "03cbcaa9c98c877a26977d00825c956a238e8dddfbd322cce4f74b0b5bd6ace4a7";*/
            const string expectedPath = "m/0/2147483647'/1";
            const string expectedChainCode = "f366f48f1ea9f2d1d3fe958c95ca84ea18e4c4ddb9366c336c927eb246fb38cb";
            const string expectedKey = "704addf544a06e5ee4bea37098463c23613da32020d604506da8c0518e1da4b7";
            const string expectedPublicKey = "03a7d1d856deb74c508e05031f9895dab54626251b3806e16b4bd12e781a7df5b9";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector2KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector2ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestSecp256k1Vector2Test4()
        {
        /*private const string Secp256k1Vector2KeyHexExpected = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
        private const string Secp256k1Vector2ChainCodeExpected = "03cbcaa9c98c877a26977d00825c956a238e8dddfbd322cce4f74b0b5bd6ace4a7";*/
            const string expectedPath = "m/0/2147483647'/1/2147483646'";
            const string expectedChainCode = "637807030d55d01f9a0cb3a7839515d796bd07706386a6eddf06cc29a65a0e29";
            const string expectedKey = "f1c7c871a54a804afe328b4c83a1c33b8e5ff48f5087273f04efa83b247d6a2d";
            const string expectedPublicKey = "02d2b36900396c9282fa14628566582f206a5dd0bcc8d5e892611806cafb0301f0";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector2KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector2ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        [Fact]
        public void TestSecp256k1Vector2Test5()
        {
        /*private const string Secp256k1Vector2KeyHexExpected = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
        private const string Secp256k1Vector2ChainCodeExpected = "03cbcaa9c98c877a26977d00825c956a238e8dddfbd322cce4f74b0b5bd6ace4a7";*/
            const string expectedPath = "m/0/2147483647'/1/2147483646'/2";
            const string expectedChainCode = "9452b549be8cea3ecb7a84bec10dcfd94afe4d129ebfd3b3cb58eedf394ed271";
            const string expectedKey = "bb7d39bdb83ecf58f2fd82b6d918341cbef428661ef01ab97c28a4842125ac23";
            const string expectedPublicKey = "024d902e1a2fc7a8755ab5b694c575fce742c48d9ff192e63df5193e4c7afe1f9c";

            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(Secp256k1Vector2KeyHexExpected, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(Secp256k1Vector2ChainCodeExpected, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }
        #endregion

        #region nist256p1

        [Fact]
        public void TestNist256p1Vector1Test1()
        {
            const string masterChainCode = "beeb672fe4621673f722f38529c07392fecaa61015c80c34f29ce8b41b3cb6ea";
            const string masterPrivate = "612091aaa12e22dd2abef664f8a01a82cae99ad7441b7ef8110424915c268bc2";
            const string expectedPath = "m/0'";
            const string expectedChainCode = "3460cea53e6a6bb5fb391eeef3237ffd8724bf0a40e94943c98b83825342ee11";
            const string expectedKey = "6939694369114c67917a182c59ddb8cafc3004e63ca5d3b84403ba8613debc0c";
            const string expectedPublicKey = "0384610f5ecffe8fda089363a41f56a5c7ffc1d81b59a612d0d649b2d22355590c";
            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);

            
            Assert.Equal(masterPrivate, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(masterChainCode, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }
        
        [Fact]
        public void TestNist256p1Vector1Test2()
        {
            //TODO TBD
            const string masterChainCode = "beeb672fe4621673f722f38529c07392fecaa61015c80c34f29ce8b41b3cb6ea";
            const string masterPrivate = "612091aaa12e22dd2abef664f8a01a82cae99ad7441b7ef8110424915c268bc2";
            const string expectedPath = "m/0'/1";
            const string expectedChainCode = "4187afff1aafa8445010097fb99d23aee9f599450c7bd140b6826ac22ba21d0c";
            const string expectedKey = "284e9d38d07d21e4e281b645089a94f4cf5a5a81369acf151a1c3a57f18b2129";
            const string expectedPublicKey = "03526c63f8d0b4bbbf9c80df553fe66742df4676b241dabefdef67733e070f6844";
            var testMasterKeyFromSeed = TestMasterKeyFromSeed(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);

            
            Assert.Equal(masterPrivate, Hex.Convert(testMasterKeyFromSeed.Key));
            Assert.Equal(masterChainCode, Hex.Convert(testMasterKeyFromSeed.ChainCode));

            var testDerivePath = TestDerivePath(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);
            Assert.Equal(expectedKey, Hex.Convert(testDerivePath.Key));
            Assert.Equal(expectedChainCode, Hex.Convert(testDerivePath.ChainCode));
            
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);
            Assert.Equal(expectedPublicKey, Hex.Convert(testPublicKey));
        }

        #endregion
        
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
        //TODO TestVector3
        //TODO TestVector4
    }
}