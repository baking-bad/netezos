using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Forging.Models;
using Xunit;
using Dynamic.Json;

namespace Netezos.Tests.Forging
{
    public class LocalForgingTest
    {
        [Fact]
        public async Task CompareToFile()
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
                            "delegation" =>(DelegationContent)json.contents[0],
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
    }
}