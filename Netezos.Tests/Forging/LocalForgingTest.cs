using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Netezos;
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

        [Fact]
        public async Task CompareUnforgedFromFile()
        {
            //string opHex = @"../../../Forging/operations/ongeyECmEtrDFJbebtL1Frq4HPp369bvcTBZaL6kR4FsF36r72r/forged.hex";

            //byte[] localBytes = Hex.Parse(opHex);

            //Span<byte> operationBytes = localBytes.GetSpan(32);

            LocalForge localForge = new LocalForge();

            const string BRANCH = "BLnDza87LjJH4WHp7hkn77ADLoFQNrHX6KojP6dnTf12BpvwS3r";

            BallotContent content = new BallotContent
            {
                Period = 19,
                Source = "tz1UGDXghfW4Z7UhobBkfQTayMrVssCgsGGQ",
                Proposal = "PsBABY5HQTSkA4297zNHfsZNKtxULfL18y95qb3m53QJiXGmrbU",
                Ballot = Ballot.Yay
            };

            //ProposalsContent content = new ProposalsContent
            //{
            //    Period = 19,
            //    Source = "tz1go7f6mEQfT2xX2LuHAqgnRGN6c2zHPf5c",
            //    Proposals = new List<string>
            //    {
            //        "PtCarthavAMoXqbjBPVgDCRd5LgT7qqKWUPXnYii3xCaHRBMfHH",
            //        "PtEdoTezd3RHSC31mpxxo1npxFjoWWcFgQtxapi51Z8TLu6v6Uq",
            //    }
            //};

            byte[] op = await localForge.ForgeOperationAsync(BRANCH, content);
            (string branch, OperationContent oc) = await localForge.UnforgeOperationAsync(op);

            //MichelineArray ma = new MichelineArray(2);
            //MichelineArray ima = new MichelineArray(1);
            //ima.Add(new MichelineInt(5));
            //ma.Add(ima);
            //ma.Add(new MichelineString("We love Tezos!"));
            //ma.Add(new MichelineString("We love Tezos!"));
            //ma.Add(new MichelineString("We love Tezos!"));
            //ma.Add(new MichelineString("We love Tezos!"));
            //ma.Add(new MichelineInt(15));
            //byte[] opBytes = LocalForge.ForgeMicheline(ma);
            //var result = LocalForge.UnforgeMicheline(opBytes);

            //OriginationContent content = await localForge.ParseOperationAsync<OriginationContent>(operationBytes);

            await Task.FromResult(0);

            //var directories = Directory.GetDirectories(basePath);
            //foreach (var directory in directories)
            //{
            //    var json = DJson.Read($"{directory}/unsigned.json", options);
            //    switch ((int)json.contents.count)
            //    {
            //        case int n when (n > 1):
            //            var managerOps = ((IEnumerable<dynamic>)json.contents)
            //                .Select(x => (string)x.kind switch
            //                {
            //                    "origination" => (ManagerOperationContent)(OriginationContent)x,
            //                    "reveal" => (RevealContent)x,
            //                    "transaction" => (TransactionContent)x,
            //                    "delegation" => (DelegationContent)x,
            //                    _ => throw new ArgumentException("Unknown content type")
            //                })
            //                .ToList();

            //            var opByt = File.ReadAllText($"{directory}/forged.hex");

            //            var localByt = await localForge.ForgeOperationGroupAsync(json.branch, managerOps);
            //            Assert.True(opByt == Hex.Convert(localByt), $"{directory}");
            //            break;
            //        case int n when (n == 1):
            //            var c = (string)json.contents[0].kind switch
            //            {
            //                "origination" => (OperationContent)(OriginationContent)json.contents[0],
            //                "reveal" => (RevealContent)json.contents[0],
            //                "transaction" => (TransactionContent)json.contents[0],
            //                "delegation" => (DelegationContent)json.contents[0],
            //                "activate_account" => (ActivationContent)json.contents[0],
            //                "double_baking_evidence" => (DoubleBakingContent)json.contents[0],
            //                "endorsement" => (EndorsementContent)json.contents[0],
            //                "seed_nonce_revelation" => (SeedNonceRevelationContent)json.contents[0],
            //                "proposals" => (ProposalsContent)json.contents[0],
            //                "ballot" => (BallotContent)json.contents[0],
            //                "double_endorsement_evidence" => (DoubleEndorsementContent)json.contents[0],
            //                _ => throw new ArgumentException("Unknown type")
            //            };

            //            var opByte = File.ReadAllText($"{directory}/forged.hex");
            //            var localByte = await localForge.ForgeOperationAsync(json.branch, c);
            //            Assert.True(opByte == Hex.Convert(localByte), $"{directory}");
            //            break;
            //        default:
            //            throw new ArgumentException();
            //    }
            //}
        }
    }
}
