using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Forge.Operations;
using Netezos.Forge.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Netezos.Forge.Tests
{
    public class LocalForgingTest
    {
        [Fact]
        public async Task CompareToFile()
        {
            var localForge = new LocalForge();
            var settings = new JsonSerializerSettings {
                DateParseHandling = DateParseHandling.None
            };
            
            var basePath = @"../../../operations";
            var directories = Directory.GetDirectories(basePath);
            foreach (var directory in directories)
            {
                var jsonString = File.ReadAllText($"{directory}/unsigned.json");
                var json = JsonConvert.DeserializeObject<JToken>(jsonString, settings);
                switch (json["contents"].Count())
                {
                    case int n when (n > 1):
                        var managerOps = json["contents"]
                            .Select(cont => cont["kind"].Value<string>() switch
                            {
                                "origination" => (ManagerOperationContent) cont.ToObject<OriginationContent>(),
                                "reveal" => cont.ToObject<RevealContent>(),
                                "transaction" => cont.ToObject<TransactionContent>(),
                                "delegation" => cont.ToObject<DelegationContent>(),
                                _ => throw new ArgumentException($"Unknown type {json["contents"][0]["kind"]}")
                            })
                            .ToList();

                        var opByt = File.ReadAllText($"{directory}/forged.hex");

                        var localByt = await localForge.ForgeOperationGroupAsync(json["branch"].Value<string>(), managerOps);
                        Assert.True(opByt == Hex.Convert(localByt), $"{directory}");
                        break;
                    case int n when (n == 1):
                        var c = json["contents"][0]["kind"].Value<string>() switch
                        {
                            "origination" => (OperationContent) json["contents"][0].ToObject<OriginationContent>(),
                            "reveal" => json["contents"][0].ToObject<RevealContent>(),
                            "transaction" => json["contents"][0].ToObject<TransactionContent>(),
                            "delegation" => json["contents"][0].ToObject<DelegationContent>(),
                            "activate_account" => json["contents"][0].ToObject<ActivationContent>(),
                            "double_baking_evidence" => json["contents"][0].ToObject<DoubleBakingEvidenceContent>(),
                            "endorsement" => json["contents"][0].ToObject<EndorsementContent>(),
                            "seed_nonce_revelation" => json["contents"][0].ToObject<SeedNonceRevelationContent>(),
                            "proposals" => json["contents"][0].ToObject<ProposalsContent>(),
                            "ballot" => json["contents"][0].ToObject<BallotContent>(),
                            "double_endorsement_evidence" => json["contents"][0]
                                .ToObject<DoubleEndorsementEvidenceContent>(),
                            _ => throw new ArgumentException($"Unknown type {json["contents"][0]["kind"]}")
                        };
                        
                        var opByte = File.ReadAllText($"{directory}/forged.hex");
                        var localByte = await localForge.ForgeOperationAsync(json["branch"].Value<string>(), c);
                        Assert.True(opByte == Hex.Convert(localByte), $"{directory}");
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }
}