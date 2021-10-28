using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Dynamic.Json;
using Xunit;

using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Forging.Models;

namespace Netezos.Tests.Forging
{
    public class LocalForgingTest
    {
        [Fact]
        public async Task CompareForgedToFile()
        {
            var forge = new LocalForge();
            var options = new JsonSerializerOptions { MaxDepth = 10240 };

            var directories = Directory.GetDirectories("../../../Forging/operations");
            foreach (var directory in directories)
            {
                var op = (Operation)DJson.Read($"{directory}/unsigned.json", options);
                var opBytes = File.ReadAllText($"{directory}/forged.hex");
                var localBytes = op.Contents.Count == 1
                    ? await forge.ForgeOperationAsync(op.Branch, op.Contents[0])
                    : await forge.ForgeOperationGroupAsync(op.Branch, op.Contents.Select(x => x as ManagerOperationContent));

                Assert.True(opBytes == Hex.Convert(localBytes), directory);
            }
        }

        [Fact]
        public async Task CompareUnforgedFromFile()
        {
            var forge = new LocalForge();
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                MaxDepth = 10240,
            };
            options.Converters.Add(new DateTimUtcTimezoneAppender());

            var jsonComparer = new JsonElementComparer(true, true);
            var directories = Directory.GetDirectories("../../../Forging/operations");

            JsonElement toJsonElement(object o)
            {
                // JSON serialize the unforged manager operation for the purpose of deserializing as JsonElement for comparison
                var serMop = JsonSerializer.Serialize(o, o.GetType(), options);
                // Deserialize the unforged manager operation to JsonElement for comparison
                return (JsonElement)DJson.Parse(serMop);
            }

            foreach (var directory in directories)
            {
                var json = DJson.Read($"{directory}/unsigned.json", options);

                // Parse the op bytes
                var opBytes = Hex.Parse(File.ReadAllText($"{directory}/forged.hex"));

                // Unforge the op bytes
                var op = await forge.UnforgeOperationAsync(opBytes);

                // Serialize/deserialize each operation for the purpose of conversion to JsonElement for comparison
                var deserOps = op.Item2.Select(toJsonElement);

                // Assert branch
                Assert.Equal(json.branch, op.Item1);
                // Assert equivalent JSON operations
                Assert.Equal(json.contents.count, op.Item2.Count());
                Assert.True(((JsonElement)json.contents).EnumerateArray().SequenceEqual(deserOps, jsonComparer));
            }
        }
    }
}
