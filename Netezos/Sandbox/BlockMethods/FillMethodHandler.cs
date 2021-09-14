using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.BlockMethods
{
    public class FillMethodHandler : BlockMethodHandler
    {
        public FillMethodHandler(TezosRpc rpc,
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
            values.Counter = values.Counter == 0 
                ? await Rpc.Blocks.Head.Context.Contracts[source].Counter.GetAsync<int>() + 1
                : values.Counter;

            values.Operations = values.Operations.Select(FillConstants).ToList();

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

        private OperationContent FillConstants(OperationContent operation)
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
                    activation.Address = Values.Key.PubKey.Address;
                    activation.Secret = "7375ef222cc038001b6c8fb768246c86e994745b";
                    return activation;
                case DoubleBakingContent doubleBaking:
                    return doubleBaking;
                case DoubleEndorsementContent doubleEndorsement:
                    return doubleEndorsement;
                case SeedNonceRevelationContent seed:
                    return seed;
                case DelegationContent delegation:
                    delegation.Delegate = Values.Key.PubKey.Address;
                    delegation.Source = Values.Key.PubKey.Address;
                    delegation.Counter = ++Values.Counter;
                    delegation.GasLimit = delegation.DefaultGasLimit();
                    delegation.StorageLimit = delegation.DefaultStorageLimit();
                    return delegation;
                case OriginationContent origination:
                    origination.Delegate = Values.Key.PubKey.Address;
                    origination.Source = Values.Key.PubKey.Address;
                    origination.Counter = ++Values.Counter;
                    origination.GasLimit = origination.DefaultGasLimit();
                    origination.StorageLimit = origination.DefaultStorageLimit();
                    return origination;
                case TransactionContent transaction:
                    transaction.Fee = transaction.DefaultFee();
                    transaction.Source = Values.Key.PubKey.Address;
                    transaction.Counter = ++Values.Counter;
                    transaction.GasLimit = transaction.DefaultGasLimit();
                    transaction.StorageLimit = transaction.DefaultStorageLimit();
                    return transaction;
                case RevealContent reveal:
                    reveal.PublicKey = Values.Key.PubKey.Address;
                    reveal.Source = Values.Key.PubKey.Address;
                    reveal.Fee = reveal.DefaultFee();
                    reveal.Counter = ++Values.Counter;
                    reveal.GasLimit = reveal.DefaultGasLimit();
                    reveal.StorageLimit = reveal.DefaultStorageLimit();
                    return reveal;
                default:
                    throw new ArgumentException($"Invalid operation content kind {operation.Kind}");
            }
        }
    }
}