using System.Text.Json;
using System.Text.Json.Serialization;
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
            var options = new JsonSerializerOptions { MaxDepth = 100_000 };

            var directories = Directory.GetDirectories("../../../Forging/operations");
            foreach (var directory in directories)
            {
                var op = DJson.Read($"{directory}/unsigned.json", options);
                var opBranch = (string)op.branch;
                var opContents = (List<OperationContent>)op.contents;

                var opBytes = File.ReadAllText($"{directory}/forged.hex");
                var localBytes = opContents.Count == 1
                    ? await forge.ForgeOperationAsync(opBranch, opContents[0])
                    : await forge.ForgeOperationGroupAsync(opBranch, opContents.Select(x => (x as ManagerOperationContent)!));

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
                MaxDepth = 100_000,
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
                var (opBranch, opContents) = await forge.UnforgeOperationAsync(opBytes);

                // Assert branch
                Assert.Equal(json.branch, opBranch);
                // Assert equivalent JSON operations
                Assert.Equal(json.contents.count, opContents.Count());
                Assert.True(((JsonElement)json.contents)
                    .EnumerateArray()
                    .SequenceEqual(opContents.Select(toJsonElement), jsonComparer));
            }
        }
    }
}
