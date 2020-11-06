using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Keys;
using Netezos.Rpc;
using System.Text.Json;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            var publicKeyHashesPath = @"C:\Users\lesha\OneDrive\Рабочий стол\Baking Bad\keys\public_key_hashs";
            var secretKeysPath = @"C:\Users\lesha\OneDrive\Рабочий стол\Baking Bad\keys\secret_keys";

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
    }
}
