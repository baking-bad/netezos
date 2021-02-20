using Dynamic.Json;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Forging.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Netezos.Tests.Forging
{
    public class LocalForgingTest
    {
        [Fact]
        public async Task CompareForgedToFile()
        {
            var localForge = new LocalForge();
            var options = new JsonSerializerOptions
            {
                MaxDepth = 1024,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            var basePath = @"../../../Forging/operations";
            var directories = Directory.GetDirectories(basePath);
            foreach (var directory in directories)
            {
                var json = DJson.Read($"{directory}/unsigned.json", options);
                switch ((int)json.contents.count)
                {
                    case int n when (n > 1):
                        var managerOps = ((IEnumerable<dynamic>)json.contents)
                            .Select(x => (string)x.kind switch
                            {
                                "origination" => (ManagerOperationContent)(OriginationContent)x,
                                "reveal" => (RevealContent)x,
                                "transaction" => (TransactionContent)x,
                                "delegation" => (DelegationContent)x,
                                _ => throw new ArgumentException("Unknown content type")
                            })
                            .ToList();

                        var opByt = File.ReadAllText($"{directory}/forged.hex");

                        var localByt = await localForge.ForgeOperationGroupAsync(json.branch, managerOps);
                        Assert.True(opByt == Hex.Convert(localByt), $"{directory}");
                        break;
                    case int n when (n == 1):
                        var c = (string)json.contents[0].kind switch
                        {
                            "origination" => (OperationContent)(OriginationContent)json.contents[0],
                            "reveal" => (RevealContent)json.contents[0],
                            "transaction" => (TransactionContent)json.contents[0],
                            "delegation" => (DelegationContent)json.contents[0],
                            "activate_account" => (ActivationContent)json.contents[0],
                            "double_baking_evidence" => (DoubleBakingContent)json.contents[0],
                            "endorsement" => (EndorsementContent)json.contents[0],
                            "seed_nonce_revelation" => (SeedNonceRevelationContent)json.contents[0],
                            "proposals" => (ProposalsContent)json.contents[0],
                            "ballot" => (BallotContent)json.contents[0],
                            "double_endorsement_evidence" => (DoubleEndorsementContent)json.contents[0],
                            _ => throw new ArgumentException("Unknown type")
                        };

                        var opByte = File.ReadAllText($"{directory}/forged.hex");
                        var localByte = await localForge.ForgeOperationAsync(json.branch, c);
                        Assert.True(opByte == Hex.Convert(localByte), $"{directory}");
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        [Fact]
        public async Task CompareUnforgedFromFile()
        {
            var localForge = new LocalForge();
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                MaxDepth = 1024,
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            options.Converters.Add(new DateTimUtcTimezoneAppender());

            var jsonComparer = new JsonElementComparer(true, true);
            var basePath = @"../../../Forging/operations";
            var directories = Directory.GetDirectories(basePath);

            Func<object, JsonElement> toJsonElement = (o) =>
            {
                // JSON serialize the unforged manager operation for the purpose of deserializing as JsonElement for comparison
                var serMop = JsonSerializer.Serialize(o, o.GetType(), options);
                // Deserialize the unforged manager operation to JsonElement for comparison
                return (JsonElement)DJson.Parse(serMop);
            };

            foreach (var directory in directories)
            {
                var json = DJson.Read($"{directory}/unsigned.json", options);

                // Parse the op bytes
                var opBytes = Hex.Parse(File.ReadAllText($"{directory}/forged.hex"));

                // Unforge the op bytes
                var op = await localForge.UnforgeOperationAsync(opBytes);

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
