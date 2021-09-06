using Dynamic.Json;
using Netezos.Encoding;
using Netezos.Keys;
using Xunit;

namespace Netezos.Tests.Keys
{
    public class HDKeyTests
    {
        const string Vector1Seed = "000102030405060708090a0b0c0d0e0f";
        const string Vector2Seed = "fffcf9f6f3f0edeae7e4e1dedbd8d5d2cfccc9c6c3c0bdbab7b4b1aeaba8a5a29f9c999693908d8a8784817e7b7875726f6c696663605d5a5754514e4b484542";

        #region helpers

        static (string Key, string ChainCode) TestMasterKeyFromSeed(string seed, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var key = HDKey.FromHex(seed, hdStandard, ecKind);
            return (Hex.Convert(key.Key.GetBytes()), Hex.Convert(key.Key.GetBytes()));
        }

        static (string Key, string ChainCode) TestDerivePath(string path, string seed,HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var key = HDKey.FromHex(seed, hdStandard, ecKind);
            var derivePath = key.Derive(HDPath.Parse(path));
            return (Hex.Convert(derivePath.Key.GetBytes()), Hex.Convert(derivePath.ChainCode));
        }

        static string TestGetPublicKey(string path, string seed, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
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

            return Hex.Convert(pubKey.GetBytes());
        }

        #endregion


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
        
        #region secp256k1

        
        [Fact]
        public void TestSecp256k1Vector1Test0()
        {
            const string expectedKey = "e8f32e723decf4051aefac8e2c93c9c5b214313817cdb01a1494b917c8436b35";
            const string expectedChainCode = "873dff81c02f525623fd1fe5167eac3a55a049de3d314bb42ee227ffed37d508";
            
            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
        }
        
        [Fact]
        public void TestSecp256k1Vector1Test1()
        {
            const string expectedPath = "m/0'";
            const string expectedChainCode = "47fdacbd0f1097043b78c63c20c34ef4ed9a111d980047ad16282c7ae6236141";
            const string expectedKey = "edb2e14f9ee77d26dd93b4ecede8d16ed408ce149b6cd80b0715a2d911a0afea";
            const string expectedPublicKey = "035a784662a4a20a65bf6aab9ae98a6c068a81c52e4b032c0fb5400c706cfccc56";
            
            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector1Test2()
        {
            const string expectedPath = "m/0'/1";
            const string expectedChainCode = "2a7857631386ba23dacac34180dd1983734e444fdbf774041578e9b6adb37c19";
            const string expectedKey = "3c6cb8d0f6a264c91ea8b5030fadaa8e538b020f0a387421a12de9319dc93368";
            const string expectedPublicKey = "03501e454bf00751f24b1b489aa925215d66af2234e3891c3b21a52bedb3cd711c";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector1Test3()
        {
            const string expectedPath = "m/0'/1/2'";
            const string expectedChainCode = "04466b9cc8e161e966409ca52986c584f07e9dc81f735db683c3ff6ec7b1503f";
            const string expectedKey = "cbce0d719ecf7431d88e6a89fa1483e02e35092af60c042b1df2ff59fa424dca";
            const string expectedPublicKey = "0357bfe1e341d01c69fe5654309956cbea516822fba8a601743a012a7896ee8dc2";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }
    
        [Fact]
        public void TestSecp256k1Vector1Test4()
        {
            const string expectedPath = "m/0'/1/2'/2";
            const string expectedChainCode = "cfb71883f01676f587d023cc53a35bc7f88f724b1f8c2892ac1275ac822a3edd";
            const string expectedKey = "0f479245fb19a38a1954c5c7c0ebab2f9bdfd96a17563ef28a6a4b1a2a764ef4";
            const string expectedPublicKey = "02e8445082a72f29b75ca48748a914df60622a609cacfce8ed0e35804560741d29";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }
    
        [Fact]
        public void TestSecp256k1Vector1Test5()
        {
            const string expectedPath = "m/0'/1/2'/2/1000000000";
            const string expectedChainCode = "c783e67b921d2beb8f6b389cc646d7263b4145701dadd2161548a8b078e65e9e";
            const string expectedKey = "471b76e389e528d6de6d816857e012c5455051cad6660850e58372a6c3e6e7c8";
            const string expectedPublicKey = "022a471424da5e657499d1ff51cb43c47481a03b1e77f951fe64cec9f5a48f7011";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector1Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector2Test0()
        {
            const string expectedKey = "4b03d6fc340455b363f51020ad3ecca4f0850280cf436c70c727923f6db46c3e";
            const string expectedChainCode = "60499f801b896d83179a4374aeb7822aaeaceaa0db1f85ee3e904c4defbd9689";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
        }

        [Fact]
        public void TestSecp256k1Vector2Test1()
        {
            const string expectedPath = "m/0";
            const string expectedChainCode = "f0909affaa7ee7abe5dd4e100598d4dc53cd709d5a5c2cac40e7412f232f7c9c";
            const string expectedKey = "abe74a98f6c7eabee0428f53798f0ab8aa1bd37873999041703c742f15ac7e1e";
            const string expectedPublicKey = "02fc9e5af0ac8d9b3cecfe2a888e2117ba3d089d8585886c9c826b6b22a98d12ea";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector2Test2()
        {
            const string expectedPath = "m/0/2147483647'";
            const string expectedChainCode = "be17a268474a6bb9c61e1d720cf6215e2a88c5406c4aee7b38547f585c9a37d9";
            const string expectedKey = "877c779ad9687164e9c2f4f0f4ff0340814392330693ce95a58fe18fd52e6e93";
            const string expectedPublicKey = "03c01e7425647bdefa82b12d9bad5e3e6865bee0502694b94ca58b666abc0a5c3b";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector2Test3()
        {
            const string expectedPath = "m/0/2147483647'/1";
            const string expectedChainCode = "f366f48f1ea9f2d1d3fe958c95ca84ea18e4c4ddb9366c336c927eb246fb38cb";
            const string expectedKey = "704addf544a06e5ee4bea37098463c23613da32020d604506da8c0518e1da4b7";
            const string expectedPublicKey = "03a7d1d856deb74c508e05031f9895dab54626251b3806e16b4bd12e781a7df5b9";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector2Test4()
        {
            const string expectedPath = "m/0/2147483647'/1/2147483646'";
            const string expectedChainCode = "637807030d55d01f9a0cb3a7839515d796bd07706386a6eddf06cc29a65a0e29";
            const string expectedKey = "f1c7c871a54a804afe328b4c83a1c33b8e5ff48f5087273f04efa83b247d6a2d";
            const string expectedPublicKey = "02d2b36900396c9282fa14628566582f206a5dd0bcc8d5e892611806cafb0301f0";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }

        [Fact]
        public void TestSecp256k1Vector2Test5()
        {
            const string expectedPath = "m/0/2147483647'/1/2147483646'/2";
            const string expectedChainCode = "9452b549be8cea3ecb7a84bec10dcfd94afe4d129ebfd3b3cb58eedf394ed271";
            const string expectedKey = "bb7d39bdb83ecf58f2fd82b6d918341cbef428661ef01ab97c28a4842125ac23";
            const string expectedPublicKey = "024d902e1a2fc7a8755ab5b694c575fce742c48d9ff192e63df5193e4c7afe1f9c";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            
            var testPublicKey = TestGetPublicKey(expectedPath, Vector2Seed, HDStandardKind.Slip10, ECKind.Secp256k1);
            Assert.Equal(expectedPublicKey, testPublicKey);
        }
        
        [Fact]
        public void TestVector3()
        {
            const string seed = "4b381541583be4423346c643850da4b320e46a87ae3d2a4e6da11eba819cd4acba45d239319ac14f863b8d5ab5a0d0c64d2e8a1e7d1457df2e5a3c51c73235be";
            const string masterPrivate = "00ddb80b067e0d4993197fe10f2657a844a384589847602d56f0c629c81aae32";
            const string masterPublic = "03683af1ba5743bdfc798cf814efeeab2735ec52d95eced528e692b8e34c4e5669";
            const string masterChainCode = "01d28a3e53cffa419ec122c968b3259e16b65076495494d97cae10bbfec3c36f";
            const string expectedPath = "m/0'";
            const string expectedChainCode = "e5fea12a97b927fc9dc3d2cb0d1ea1cf50aa5a1fdc1f933e8906bb38df3377bd";
            const string expectedKey = "491f7a2eebc7b57028e0d3faa0acda02e75c33b03c48fb288c41e2ea44e1daef";
            const string expectedPublicKey = "026557fdda1d5d43d79611f784780471f086d58e8126b8c40acb82272a7712e7f2";
            
            var key = HDKey.FromHex(seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            
            Assert.Equal(masterPrivate, key.Key.GetHex());
            Assert.Equal(masterChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(masterPublic, key.PubKey.GetHex());

            var derivedKey = key.Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, derivedKey.Key.GetHex());
            Assert.Equal(expectedChainCode, derivedKey.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, derivedKey.PubKey.GetHex());
        }
        
        [Fact]
        public void TestVector4()
        {
            const string seed = "3ddd5602285899a946114506157c7997e5444528f3003f6134712147db19b678";
            const string masterPrivate = "12c0d59c7aa3a10973dbd3f478b65f2516627e3fe61e00c345be9a477ad2e215";
            const string masterPublic = "026f6fedc9240f61daa9c7144b682a430a3a1366576f840bf2d070101fcbc9a02d";
            const string masterChainCode = "d0c8a1f6edf2500798c3e0b54f1b56e45f6d03e6076abd36e5e2f54101e44ce6";
            const string expectedPath = "m/0'";
            const string expectedChainCode = "cdc0f06456a14876c898790e0b3b1a41c531170aec69da44ff7b7265bfe7743b";
            const string expectedKey = "00d948e9261e41362a688b916f297121ba6bfb2274a3575ac0e456551dfd7f7e";
            const string expectedPublicKey = "039382d2b6003446792d2917f7ac4b3edf079a1a94dd4eb010dc25109dda680a9d";
            
            var key = HDKey.FromHex(seed, HDStandardKind.Slip10, ECKind.Secp256k1);

            
            Assert.Equal(masterPrivate, key.Key.GetHex());
            Assert.Equal(masterChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(masterPublic, key.PubKey.GetHex());

            var derivedKey = key.Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, derivedKey.Key.GetHex());
            Assert.Equal(expectedChainCode, derivedKey.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, derivedKey.PubKey.GetHex());
        }
        
        [Fact]
        public void TestVector4Test3()
        {
            const string seed = "3ddd5602285899a946114506157c7997e5444528f3003f6134712147db19b678";
            const string expectedPath = "m/0'/1'";
            const string expectedChainCode = "a48ee6674c5264a237703fd383bccd9fad4d9378ac98ab05e6e7029b06360c0d";
            const string expectedKey = "3a2086edd7d9df86c3487a5905a1712a9aa664bce8cc268141e07549eaa8661d";
            const string expectedPublicKey = "032edaf9e591ee27f3c69c36221e3c54c38088ef34e93fbb9bb2d4d9b92364cbbd";
            
            var key = HDKey.FromHex(seed, HDStandardKind.Slip10, ECKind.Secp256k1).Derive(expectedPath);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        #endregion

        #region nist256p1
        
        [Fact]
        public void TestNist256p1Vector1Test1()
        {
            const string expectedChainCode = "beeb672fe4621673f722f38529c07392fecaa61015c80c34f29ce8b41b3cb6ea";
            const string expectedKey = "612091aaa12e22dd2abef664f8a01a82cae99ad7441b7ef8110424915c268bc2";
            const string expectedPublicKey = "0266874dc6ade47b3ecd096745ca09bcd29638dd52c2c12117b11ed3e458cfa9e8";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);

            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());

        }
        
        [Fact]
        public void TestNist256p1Vector1Test2()
        {
            const string expectedPath = "m/0'";
            const string expectedChainCode = "3460cea53e6a6bb5fb391eeef3237ffd8724bf0a40e94943c98b83825342ee11";
            const string expectedKey = "6939694369114c67917a182c59ddb8cafc3004e63ca5d3b84403ba8613debc0c";
            const string expectedPublicKey = "0384610f5ecffe8fda089363a41f56a5c7ffc1d81b59a612d0d649b2d22355590c";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector1Test3()
        {
            //TODO TBD
            const string expectedPath = "m/0'/1";
            const string expectedChainCode = "4187afff1aafa8445010097fb99d23aee9f599450c7bd140b6826ac22ba21d0c";
            const string expectedKey = "284e9d38d07d21e4e281b645089a94f4cf5a5a81369acf151a1c3a57f18b2129";
            const string expectedPublicKey = "03526c63f8d0b4bbbf9c80df553fe66742df4676b241dabefdef67733e070f6844";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector1Test4()
        {
            const string expectedPath = "m/0'/1/2'";
            const string expectedChainCode = "98c7514f562e64e74170cc3cf304ee1ce54d6b6da4f880f313e8204c2a185318";
            const string expectedKey = "694596e8a54f252c960eb771a3c41e7e32496d03b954aeb90f61635b8e092aa7";
            const string expectedPublicKey = "0359cf160040778a4b14c5f4d7b76e327ccc8c4a6086dd9451b7482b5a4972dda0";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector1Test5()
        {
            const string expectedPath = "m/0'/1/2'/2";
            const string expectedChainCode = "ba96f776a5c3907d7fd48bde5620ee374d4acfd540378476019eab70790c63a0";
            const string expectedKey = "5996c37fd3dd2679039b23ed6f70b506c6b56b3cb5e424681fb0fa64caf82aaa";
            const string expectedPublicKey = "029f871f4cb9e1c97f9f4de9ccd0d4a2f2a171110c61178f84430062230833ff20";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector1Test6()
        {
            const string expectedPath = "m/0'/1/2'/2/1000000000";
            const string expectedChainCode = "b9b7b82d326bb9cb5b5b121066feea4eb93d5241103c9e7a18aad40f1dde8059";
            const string expectedKey = "21c4f269ef0a5fd1badf47eeacebeeaa3de22eb8e5b0adcd0f27dd99d34d0119";
            const string expectedPublicKey = "02216cd26d31147f72427a453c443ed2cde8a1e53c9cc44e5ddf739725413fe3f4";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(HDPath.Parse(expectedPath));
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector2Test1()
        {
            const string expectedChainCode = "96cd4465a9644e31528eda3592aa35eb39a9527769ce1855beafc1b81055e75d";
            const string expectedKey = "eaa31c2e46ca2962227cf21d73a7ef0ce8b31c756897521eb6c7b39796633357";
            const string expectedPublicKey = "02c9e16154474b3ed5b38218bb0463e008f89ee03e62d22fdcc8014beab25b48fa";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.NistP256);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector2Test2()
        {
            const string expectedPath = "m/0";
            const string expectedChainCode = "84e9c258bb8557a40e0d041115b376dd55eda99c0042ce29e81ebe4efed9b86a";
            const string expectedKey = "d7d065f63a62624888500cdb4f88b6d59c2927fee9e6d0cdff9cad555884df6e";
            const string expectedPublicKey = "039b6df4bece7b6c81e2adfeea4bcf5c8c8a6e40ea7ffa3cf6e8494c61a1fc82cc";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector2Test3()
        {
            const string expectedPath = "m/0/2147483647'";
            const string expectedChainCode = "f235b2bc5c04606ca9c30027a84f353acf4e4683edbd11f635d0dcc1cd106ea6";
            const string expectedKey = "96d2ec9316746a75e7793684ed01e3d51194d81a42a3276858a5b7376d4b94b9";
            const string expectedPublicKey = "02f89c5deb1cae4fedc9905f98ae6cbf6cbab120d8cb85d5bd9a91a72f4c068c76";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);

            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector2Test4()
        {
            const string expectedPath = "m/0/2147483647'/1";
            const string expectedChainCode = "7c0b833106235e452eba79d2bdd58d4086e663bc8cc55e9773d2b5eeda313f3b";
            const string expectedKey = "974f9096ea6873a915910e82b29d7c338542ccde39d2064d1cc228f371542bbc";
            const string expectedPublicKey = "03abe0ad54c97c1d654c1852dfdc32d6d3e487e75fa16f0fd6304b9ceae4220c64";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);

            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector2Test5()
        {
            const string expectedPath = "m/0/2147483647'/1/2147483646'";
            const string expectedChainCode = "5794e616eadaf33413aa309318a26ee0fd5163b70466de7a4512fd4b1a5c9e6a";
            const string expectedKey = "da29649bbfaff095cd43819eda9a7be74236539a29094cd8336b07ed8d4eff63";
            const string expectedPublicKey = "03cb8cb067d248691808cd6b5a5a06b48e34ebac4d965cba33e6dc46fe13d9b933";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);

            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void TestNist256p1Vector2Test6()
        {
            const string expectedPath = "m/0/2147483647'/1/2147483646'/2";
            const string expectedChainCode = "3bfb29ee8ac4484f09db09c2079b520ea5616df7820f071a20320366fbe226a7";
            const string expectedKey = "bb0a77ba01cc31d77205d51d08bd313b979a71ef4de9b062f8958297e746bd67";
            const string expectedPublicKey = "020ee02e18967237cf62672983b253ee62fa4dd431f8243bfeccdf39dbe181387f";

            var key = HDKey.FromHex(Vector2Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);

            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void DerivationRetryTest1()
        {
            const string expectedChainCode = "beeb672fe4621673f722f38529c07392fecaa61015c80c34f29ce8b41b3cb6ea";
            const string expectedKey = "612091aaa12e22dd2abef664f8a01a82cae99ad7441b7ef8110424915c268bc2";
            const string expectedPublicKey = "0266874dc6ade47b3ecd096745ca09bcd29638dd52c2c12117b11ed3e458cfa9e8";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void DerivationRetryTest2()
        {
            const string expectedPath = "m/28578'";
            const string expectedChainCode = "e94c8ebe30c2250a14713212f6449b20f3329105ea15b652ca5bdfc68f6c65c2";
            const string expectedKey = "06f0db126f023755d0b8d86d4591718a5210dd8d024e3e14b6159d63f53aa669";
            const string expectedPublicKey = "02519b5554a4872e8c9c1c847115363051ec43e93400e030ba3c36b52a3e70a5b7";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void DerivationRetryTest3()
        {
            const string expectedPath = "m/28578'/33941";
            const string expectedChainCode = "9e87fe95031f14736774cd82f25fd885065cb7c358c1edf813c72af535e83071";
            const string expectedKey = "092154eed4af83e078ff9b84322015aefe5769e31270f62c3f66c33888335f3a";
            const string expectedPublicKey = "0235bfee614c0d5b2cae260000bb1d0d84b270099ad790022c1ae0b2e782efe120";

            var key = HDKey.FromHex(Vector1Seed, HDStandardKind.Slip10, ECKind.NistP256).Derive(expectedPath);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        [Fact]
        public void SeedRetryTest()
        {
            const string seed = "a7305bc8df8d0951f0cb224c0e95d7707cbdf2c6ce7e8d481fec69c7ff5e9446";
            const string expectedChainCode = "7762f9729fed06121fd13f326884c82f59aa95c57ac492ce8c9654e60efd130c";
            const string expectedKey = "3b8c18469a4634517d6d0b65448f8e6c62091b45540a1743c5846be55d47d88f";
            const string expectedPublicKey = "0383619fadcde31063d8c5cb00dbfe1713f3e6fa169d8541a798752a1c1ca0cb20";
            
            var key = HDKey.FromHex(seed, HDStandardKind.Slip10, ECKind.NistP256);
            
            Assert.Equal(expectedKey, key.Key.GetHex());
            Assert.Equal(expectedChainCode, key.ChainCode.ToStringHex());
            Assert.Equal(expectedPublicKey, key.PubKey.GetHex());
        }
        
        #endregion
        
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


        
        //TODO TestVector3
        //TODO TestVector4
    }
}