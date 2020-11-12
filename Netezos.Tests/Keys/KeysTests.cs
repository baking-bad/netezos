using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Keys;
using System.Text.Json;
using Xunit;
using System.Linq;
using Netezos.Encoding;


namespace Netezos.Tests.Keys
{

    public class KeysTests
    {
        [Fact]
        public void TestKey()
        {
            var key1 = new Key(ECKind.Ed25519);
            var key2 = new Key(ECKind.NistP256);
            var key3 = new Key(ECKind.Secp256k1);
        }

        [Fact]
        public async Task TestPublicKeyHashes()
        {
            var publicKeyHashes = await File.ReadAllTextAsync("Keys/KeysFiles/public_key_hashes.json");
            var secretKey = await File.ReadAllTextAsync("Keys/KeysFiles/secret_keys.json");
            var publicKeyHashesList = JsonSerializer.Deserialize<List<GeneratedKeys>>(publicKeyHashes);
            var secretKeysList = JsonSerializer.Deserialize<List<GeneratedKeys>>(secretKey);

            foreach (var item in publicKeyHashesList)
            {
                var objectSecretKeys = secretKeysList.FirstOrDefault(x => x.name == item.name);
                var key = Key.FromBase58(objectSecretKeys.value.Remove(0, 12));
                Assert.True(item.value == key.PubKey.Address);
            }
        }

        [Fact]
        public async Task TestPublicKeys()
        {
            var publicKeys = await File.ReadAllTextAsync("Keys/KeysFiles/public_keys.json");
            var secretKey = await File.ReadAllTextAsync("Keys/KeysFiles/secret_keys.json");
            var publicKeysList = JsonSerializer.Deserialize<List<GeneratedKeys>>(publicKeys);
            var secretKeysList = JsonSerializer.Deserialize<List<GeneratedKeys>>(secretKey);

            foreach (var item in publicKeysList)
            {
                var objectSecretKeys = secretKeysList.FirstOrDefault(x => x.name == item.name);
                var key = Key.FromBase58(objectSecretKeys.value.Remove(0, 12));
                Console.WriteLine(key.PubKey);
                Assert.True(item.value.Remove(0, 12) == key.PubKey.ToString());
            }
        }

        [Fact]
        public async Task TestSignature()
        {

            var signature = await File.ReadAllBytesAsync("Keys/KeysFiles/signature.json");
            var signatureList = JsonSerializer.Deserialize<List<SignatureKeys>>(signature);
            foreach (var item in signatureList)
            {
                Console.WriteLine(item.Value);
                var key = Key.FromBase58(item.Value);
                Assert.True(item.Signature == key.Sign(Hex.Parse("03" + item.OpBytes)));
            }
        }
    }
}
