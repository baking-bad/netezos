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

                switch ((int)json.contents.count)
                {
                    case int n when (n > 1):
                        // Parse the manager op bytes
                        var mopBytes = Hex.Parse(File.ReadAllText($"{directory}/forged.hex"));
                        // Unforge the manager op bytes
                        var mops = await localForge.UnforgeOperationGroupAsync(mopBytes);

                        // Assert branch
                        Assert.Equal(json.branch, mops.Item1);
                        // Serialize/deserialize each operation for the purpose of conversion to JsonElement for comparison.
                        IEnumerable<JsonElement> deserMops = mops.Item2.Select(toJsonElement);
                        Assert.True(((JsonElement)json.contents).EnumerateArray().SequenceEqual(deserMops, jsonComparer));
                        break;

                    case int n when (n == 1):
                        // Parse the op bytes
                        var opBytes = Hex.Parse(File.ReadAllText($"{directory}/forged.hex"));
                        // Unforge the op bytes
                        var op = await localForge.UnforgeOperationAsync(opBytes);

                        // Assert branch
                        Assert.Equal(json.branch, op.Item1);
                        // Assert equivalent JSON operations
                        Assert.True(jsonComparer.Equals((JsonElement)json.contents[0], toJsonElement(op.Item2)));
                        break;

                    default:
                        throw new ArgumentException();
                }
            }
        }
    }

    #region Testing

    //const string BRANCH = "BLnDza87LjJH4WHp7hkn77ADLoFQNrHX6KojP6dnTf12BpvwS3r";

    #region BallotContent
    //BallotContent content = new BallotContent
    //{
    //    Period = 19,
    //    Source = "tz1UGDXghfW4Z7UhobBkfQTayMrVssCgsGGQ",
    //    Proposal = "PsBABY5HQTSkA4297zNHfsZNKtxULfL18y95qb3m53QJiXGmrbU",
    //    Ballot = Ballot.Yay
    //};
    #endregion

    #region ProposalsContent
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
    #endregion

    #region ActivationContent
    //ActivationContent content = new ActivationContent
    //{
    //    Address = "tz1UDzL5bsDxyp5HqjQMSwLArjxGkYTNarLd",
    //    Secret = "7a733f0561adf4067144c297f8bbcfbe0ae2d142"
    //};
    #endregion

    #region RevealContent
    //RevealContent content = new RevealContent
    //{
    //    Source = "tz1NF7b38uQ43N4nmTHvDKpr1Qo5LF9iYawk",
    //    Fee = 1269,
    //    Counter = 390929,
    //    GasLimit = 10100,
    //    StorageLimit = 0,
    //    PublicKey = "edpkuw2nHYNcksmy2GK6xtG8R2iyHCC35jc8K1684Mc7SFjqZzch2a"
    //};
    #endregion

    #region DoubleEndorsementContent
    //DoubleEndorsementContent content = new DoubleEndorsementContent
    //{
    //    Op1 = new InlinedEndorsement
    //    {
    //        Branch = "BLyQHMFeNzZEKHmKgfD9imcowLm8hc4aUo16QtYZcS5yvx7RFqQ",
    //        Operations = new EndorsementContent
    //        {
    //            Level = 554811
    //        },
    //        Signature = "sigqgQgW5qQCsuHP5HhMhAYR2HjcChUE7zAczsyCdF681rfZXpxnXFHu3E6ycmz4pQahjvu3VLfa7FMCxZXmiMiuZFQS4MHy"
    //    },
    //    Op2 = new InlinedEndorsement
    //    {
    //        Branch = "BLTfU3iAfPFMuHTmC1F122AHqdhqnFTfkxBmzYCWtCkBMpYNjxw",
    //        Operations = new EndorsementContent
    //        {
    //            Level = 554811
    //        },
    //        Signature = "sigPwkrKhsDdEidvvUgEEtsaVhyiGmzhCYqCJGKqbYMtH8KxkrFds2HmpDCpRxSTnehKoSC8XKCs9eej6PEzcZoy6fqRAPEZ"
    //    }
    //};
    #endregion

    #region DoubleBakingContent
    //DoubleBakingContent content = new DoubleBakingContent
    //{
    //    BlockHeader1 = new BlockHeader
    //    {
    //        Level = 671732,
    //        Proto = 5,
    //        Predecessor = "BMUMkLvbHfLCRpQ8gs2o8ds8ViyNFv4jVC2cZfd9xkhRYjofzBB",
    //        Timestamp = DateTime.Parse("2019-10-30T02:36:57Z"),
    //        ValidationPass = 4,
    //        OperationsHash = "LLoa8pBdeFQqqNXaY8gEqJiszWMhN9t1QwhBVGRaAKfPf9HboS7Mo",
    //        Fitness = new List<string> { "01", "0000000000003ff4" },
    //        Context = "CoVKugCtBgDvmYP1R73Yw4huXghtZSLXkfzdctvFLU2bEDXqexbR",
    //        Priority = 1,
    //        ProofOfWorkNonce = "00bc030300027531",
    //        Signature = "sigibxc9MH3tNdiMT6LAEhbH11aREv34eHiyQFF1cnBu7YsHXRzGj9JB2ys5fgsRDkiJcJPXCfx1G2kVVFpogEdXe5tZoQhW"
    //    },
    //    BlockHeader2 = new BlockHeader
    //    {
    //        Level = 671732,
    //        Proto = 5,
    //        Predecessor = "BMUMkLvbHfLCRpQ8gs2o8ds8ViyNFv4jVC2cZfd9xkhRYjofzBB",
    //        Timestamp = DateTime.Parse("2019-10-30T02:36:57Z"),
    //        ValidationPass = 4,
    //        OperationsHash = "LLoaiqFM8FMEsPBvcDh6rF6wXH88dTGVQ1SQqBjHp6R8EhTki52T4",
    //        Fitness = new List<string> { "01", "0000000000003ff4" },
    //        Context = "CoWCXmo4y4GGLijFoJ5f8pj75hmxhyZWWGgdeG7WWiRXrmK9gTfi",
    //        Priority = 1,
    //        ProofOfWorkNonce = "a7d357bb0c8e0e00",
    //        Signature = "sigscjExjrUHrYgeaT1WWKZxYuPSTNfB7cgBkuRhqxWXzYfjrHs8qi9gerqU5w9ia2tiSnb9agkdPrSPpu7rPa6suP9omtNT"
    //    }
    //};
    #endregion

    #region SeedNonceRevelationContent
    //SeedNonceRevelationContent content = new SeedNonceRevelationContent
    //{
    //    Level = 670624,
    //    Nonce = "6daab893c8336b48517b0d8bb51cca32c621fff0dc0f4ce38d32407a0bb440ff"
    //};
    #endregion

    #region DelegationContent
    //DelegationContent content = new DelegationContent
    //{
    //    Source = "tz1iD5nmudc4QtfNW14WWaiP7JEDuUHnbXuv",
    //    Fee = 1160,
    //    Counter = 6,
    //    GasLimit = 10100,
    //    StorageLimit = 0,
    //    Delegate = "tz1aWXP237BLwNHJcCD4b3DutCevhqq2T1Z9"
    //};
    #endregion

    #region OriginationContent
    //OriginationContent content = new OriginationContent
    //{
    //    Source = "tz1TJCwoX79reCZ8yccPeW8iB9Mba91v8H47",
    //    Fee = 1389,
    //    Counter = 307028,
    //    GasLimit = 11140,
    //    StorageLimit = 323,
    //    Balance = 0,
    //    Delegate = null,
    //    Script = new Script
    //    {
    //        Code = new MichelineArray
    //        {
    //            new MichelinePrim
    //            {
    //                Prim = PrimType.parameter,
    //                Args = new MichelineArray
    //                {
    //                    new MichelinePrim
    //                    {
    //                        Prim = PrimType.unit,
    //                        Annots = new List<IAnnotation> { new FieldAnnotation("abc") }
    //                    }
    //                }
    //            },
    //            new MichelinePrim
    //            {
    //                Prim = PrimType.storage,
    //                Args = new MichelineArray
    //                {
    //                    new MichelinePrim { Prim = PrimType.unit }
    //                }
    //            },
    //            new MichelinePrim
    //            {
    //                Prim = PrimType.code,
    //                Args = new MichelineArray
    //                {
    //                    new MichelineArray
    //                    {
    //                        new MichelinePrim { Prim = PrimType.CDR },
    //                        new MichelinePrim { Prim = PrimType.NIL, Args = new MichelineArray { new MichelinePrim { Prim = PrimType.operation } } },
    //                        new MichelinePrim { Prim = PrimType.PAIR }
    //                    }
    //                }
    //            },
    //        },
    //        Storage = new MichelinePrim { Prim = PrimType.Unit }
    //    }
    //};
    #endregion

    #region TransactionContent
    //TransactionContent content = new TransactionContent
    //{
    //    Source = "tz1SJJY253HoEda8PS5vvfHVtyghgK3CTS2z",
    //    Fee = 2966,
    //    Counter = 133558,
    //    GasLimit = 26271,
    //    StorageLimit = 0,
    //    Amount = 0,
    //    Destination = "KT1XdCkJncWfGvqf1NdbK2HBRTvRcHhJtNx5",
    //    Parameters = new Parameters
    //    {
    //        Entrypoint = "do",
    //        Value = new MichelineArray
    //        {
    //            new MichelinePrim(PrimType.RENAME),
    //            new MichelinePrim(PrimType.NIL) { Args = new MichelineArray { new MichelinePrim(PrimType.operation) } },
    //            new MichelinePrim(PrimType.PUSH) { Args = new MichelineArray { new MichelinePrim(PrimType.key_hash), new MichelineString("tz2L2HuhaaSnf6ShEDdhTEAr5jGPWPNwpvcB") } },
    //            new MichelinePrim(PrimType.IMPLICIT_ACCOUNT),
    //            new MichelinePrim(PrimType.PUSH) { Args = new MichelineArray { new MichelinePrim(PrimType.mutez), new MichelineInt(2) } },
    //            new MichelinePrim(PrimType.UNIT),
    //            new MichelinePrim(PrimType.TRANSFER_TOKENS),
    //            new MichelinePrim(PrimType.CONS),
    //            new MichelinePrim(PrimType.DIP) { Args = new MichelineArray { new MichelinePrim { Prim = PrimType.DROP } } }
    //        }
    //    }
    //};
    #endregion

    #endregion
}
