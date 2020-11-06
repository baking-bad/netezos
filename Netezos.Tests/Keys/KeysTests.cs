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
        string publicKeyHashesPath = @"C:\Users\lesha\OneDrive\Рабочий стол\Baking Bad\keys\public_key_hashs";
        string secretKeysPath = @"C:\Users\lesha\OneDrive\Рабочий стол\Baking Bad\keys\secret_keys";
        string publicKeysPath = @"C:\Users\lesha\OneDrive\Рабочий стол\Baking Bad\keys\public_keys";

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
            var addressPkH = await File.ReadAllTextAsync(publicKeyHashesPath);
            var addressSkP = await File.ReadAllTextAsync(secretKeysPath);
            var publicKeyHashesList = JsonSerializer.Deserialize<List<GeneratedKeys>>(addressPkH);
            var secretKeysList = JsonSerializer.Deserialize<List<GeneratedKeys>>(addressSkP);

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
            var addressPk = await File.ReadAllTextAsync(publicKeysPath);
            var addressSkP = await File.ReadAllTextAsync(secretKeysPath);
            var publicKeysList = JsonSerializer.Deserialize<List<GeneratedKeys>>(addressPk);
            var secretKeysList = JsonSerializer.Deserialize<List<GeneratedKeys>>(addressSkP);

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
            var signaturePath = @"C:\Users\lesha\OneDrive\Рабочий стол\Baking Bad\signature.txt";
            var addressSign = await File.ReadAllTextAsync(signaturePath);
            var signatureList = JsonSerializer.Deserialize<List<SignatureKeys>>(addressSign);

            foreach (var item in signatureList)
            {
                Console.WriteLine(item.Value);
                var key = Key.FromBase58(item.Value);
                Assert.True(item.Signature == key.Sign(Hex.Parse("03" + item.OpBytes)));
            }
        }
    }
}
