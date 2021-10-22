using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Models;
using Org.BouncyCastle.Security;

namespace Netezos.Sandbox.BlockMethods
{
    public class FillMethodHandler : BlockMethodHandler
    {
        internal FillMethodHandler(TezosRpc rpc,
            BlockParameters parameters,
            Func<BlockParameters, Task<ForwardingParameters>> function = null) 
            : base(rpc, parameters, function)
        { }

        public SignMethodHandler Sign => new SignMethodHandler(Rpc, Values, CallAsync);

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(BlockParameters values)
        {
            var numberOfBlocksWait = values.BranchOffset.HasValue 
                ? SandboxParameters.MAX_OPERATIONS_TTL - values.BranchOffset 
                : values.Ttl;
            numberOfBlocksWait = numberOfBlocksWait ?? SandboxParameters.GetOperationsTtl(values.IsSandbox);

            if (numberOfBlocksWait <= 0 || numberOfBlocksWait > SandboxParameters.MAX_OPERATIONS_TTL)
                throw new ArgumentOutOfRangeException($"Ttl has to be in range (0, {SandboxParameters.MAX_OPERATIONS_TTL}]");

            values.ChainId = await Rpc.ChainId.GetAsync<string>();

            values.Branch = await Rpc.Blocks[$"head-{SandboxParameters.MAX_OPERATIONS_TTL - numberOfBlocksWait}"].Hash.GetAsync<string>();

            var shellHeader = await Rpc.Blocks.Head.Header.GetAsync<Dictionary<string, JsonElement>>();

            values.Protocol = shellHeader["protocol"].GetString();
            var source = values.Key.PubKey.Address;
            var counter = values.Counter == 0 
                ? await Rpc.Blocks.Head.Context.Contracts[source].Counter.GetAsync<int>() + 1
                : values.Counter;

            var constants = await Rpc.Blocks[values.Branch].Context.Constants.GetAsync<ConstantsContent>();
            values.Operations = values.Operations.Select(x => FillConstants(x, constants, ref counter)).ToList();

            var parameters = new ForwardingParameters()
            {
                Operations = new List<List<MempoolOperation>>()
                {
                    new List<MempoolOperation>()
                    {
                        new MempoolOperation()
                        {
                            Branch = values.Branch,
                            Contents = values.Operations,
                            Protocol = values.Protocol
                        }
                    }
                }
            };
            return parameters;
        }

        private OperationContent FillConstants(OperationContent operation, ConstantsContent constants, ref int counter)
        {
            switch (operation)
            {
                case EndorsementContent endorsement:
                    return endorsement;
                case BallotContent ballot:
                    ballot.Period =
                        int.Parse(Rpc.Blocks.Head.Metadata.GetAsync<Dictionary<string, Dictionary<string, string>>>()
                            .Result["level"]["voting_period"]);
                    ballot.Source = Values.Key.PubKey.Address;
                    return ballot;
                case ProposalsContent proposals:
                    proposals.Period = int.Parse(Rpc.Blocks.Head.Metadata.GetAsync<Dictionary<string, Dictionary<string, string>>>()
                        .Result["level"]["voting_period"]);
                    proposals.Source = Values.Key.PubKey.Address;
                    return proposals;
                case ActivationContent activation:
                    var key = Values.Key as CommitmentKey;
                    if (key == null) 
                        throw new InvalidKeyException($"Key {Values.Key.GetBase58()} is not commitment");
                    activation.Address = key.PubKey.Address;
                    activation.Secret = key.ActivationCode; 
                    return activation;
                case DoubleBakingContent doubleBaking:
                    return doubleBaking;
                case DoubleEndorsementContent doubleEndorsement:
                    return doubleEndorsement;
                case SeedNonceRevelationContent seed:
                    return seed;
                case DelegationContent delegation:
                    delegation.Delegate = Values.Key.PubKey.Address;
                    delegation.Source =  Values.Key.PubKey.Address;
                    delegation.Counter = counter++;
                    delegation.GasLimit = delegation.DefaultGasLimit(constants);
                    delegation.StorageLimit = delegation.DefaultStorageLimit(constants);
                    return delegation;
                case OriginationContent origination:
                    origination.Delegate =  Values.Key.PubKey.Address;
                    origination.Source =  Values.Key.PubKey.Address;
                    origination.Counter = counter++;
                    origination.GasLimit = origination.DefaultGasLimit(constants);
                    origination.StorageLimit = origination.DefaultStorageLimit(constants);
                    return origination;
                case TransactionContent transaction:
                    transaction.Source = Values.Key.PubKey.Address;
                    transaction.Counter = counter++;
                    transaction.GasLimit = 1427;//transaction.DefaultGasLimit(constants);
                    transaction.StorageLimit = 257;//transaction.DefaultStorageLimit(constants);
                    transaction.Fee = 407;//transaction.DefaultFee(constants);
                    return transaction;
                case RevealContent reveal:
                    reveal.PublicKey = Values.Key.PubKey.GetBase58();
                    reveal.Source = Values.Key.PubKey.Address;
                    reveal.Fee = reveal.DefaultFee(constants);
                    reveal.Counter = counter++;
                    reveal.GasLimit = reveal.DefaultGasLimit(constants);
                    reveal.StorageLimit = reveal.DefaultStorageLimit(constants);
                    return reveal;
                default:
                    throw new ArgumentException($"Invalid operation content kind {operation.Kind}");
            }
        }
    }
}