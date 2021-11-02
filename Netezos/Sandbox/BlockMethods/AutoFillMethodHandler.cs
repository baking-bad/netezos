using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox.BlockMethods
{
    public class AutoFillMethodHandler : BlockMethodHandler
    {
        internal AutoFillMethodHandler(TezosRpc rpc, 
            BlockParameters headerParameters,
            Func<BlockParameters, Task<ForwardingParameters>> function = null)
            : base(rpc, headerParameters, function)
        { }

        public SignMethodHandler Sign => new SignMethodHandler(Rpc, Values, CallAsync);

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(BlockParameters values)
        {
            var result = await new FillMethodHandler(Rpc, Values).CallAsync();

            var opgWithMetadata = await RunOperation();

            values.Operations = opgWithMetadata
                .GetProperty("contents")
                .EnumerateArray()
                .Select((x, index) => 
                    new {
                        Result = x
                            .GetProperty("metadata")
                            .TryGetProperty("operation_result", out var res) 
                                ? JsonSerializer.Deserialize<OperationResult>(res.ToString())
                                : null,
                        Index = index
                    }
                )
                .Select(x => FillConstants(values.Operations[x.Index], x.Result))
                .ToList();
            
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

        private OperationContent FillConstants(OperationContent operation, OperationResult result = null)
        {
            if (result == null)
                return operation;

            if (!string.IsNullOrEmpty(result.Status) && !result.Status.Equals("applied"))
                throw new InternalErrorException($"Operation {operation.Kind} is not valid");

            if (operation.ValidationGroup != 3)
                return operation;

            int extraSize = 96 / Values.Operations.Count + 1;

            switch (operation)
            {
                case DelegationContent delegation:
                    delegation.GasLimit = result.ConsumedGas;
                    delegation.Fee = OperationExtensions.CalculateFee(delegation, delegation.GasLimit, extraSize);

                    delegation.StorageLimit = result.PaidStorageSizeDiff
                                              + (result.AllocatedDestinationContract > 0 || result.OriginatedContracts != null ? 257 : 0);
                    return delegation;
                case OriginationContent origination:
                    origination.GasLimit = result.ConsumedGas + Values.Constants.GasReserve;
                    origination.Fee = OperationExtensions.CalculateFee(origination, origination.GasLimit, extraSize);

                    origination.StorageLimit = result.PaidStorageSizeDiff 
                                               + Values.Constants.BurnReserve
                                               + (result.AllocatedDestinationContract > 0 || result.OriginatedContracts != null ? 257 : 0);
                    return origination;
                case TransactionContent transaction:
                    transaction.GasLimit = result.ConsumedGas + Values.Constants.GasReserve;
                    transaction.Fee = OperationExtensions.CalculateFee(transaction, transaction.GasLimit, extraSize);
                    transaction.StorageLimit = result.PaidStorageSizeDiff 
                                               + Values.Constants.BurnReserve
                                               + (result.AllocatedDestinationContract > 0 || result.OriginatedContracts != null ? 257 : 0);
                    return transaction;
                case RevealContent reveal:
                    reveal.GasLimit = result.ConsumedGas;
                    reveal.Fee = OperationExtensions.CalculateFee(reveal, reveal.GasLimit, extraSize);
                    reveal.StorageLimit = result.PaidStorageSizeDiff
                                          + (result.AllocatedDestinationContract > 0 || result.OriginatedContracts != null ? 257 : 0);
                    return reveal;
                default:
                    throw new ArgumentException($"Invalid operation content kind {operation.Kind}");
            }
        }

        public class OperationResult
        {
            [JsonPropertyName("status")]
            public string Status { get; set; }
            [JsonPropertyName("consumed_gas")]
            [JsonConverter(typeof(Int32StringConverter))]
            public int ConsumedGas { get; set; }
            [JsonPropertyName("consumed_miligas")]
            [JsonConverter(typeof(Int32StringConverter))]
            public int ConsumedMiligas { get; set; }
            [JsonPropertyName("paid_storage_size_diff")]
            [JsonConverter(typeof(Int32StringConverter))]
            public int PaidStorageSizeDiff { get; set; }
            [JsonPropertyName("originated_contracts")]
            public string OriginatedContracts { get; set; }
            [JsonPropertyName("allocated_destination_contract")]
            [JsonConverter(typeof(Int32StringConverter))]
            public int AllocatedDestinationContract { get; set; }
            [JsonPropertyName("result")]
            public string Result { get; set; }
        }

        private async Task<JsonElement> RunOperation(string blockId = "head")
        {
            return await Rpc.Blocks[blockId].Helpers.Scripts.RunOperation
                .PostAsync<JsonElement>(Values.ChainId, Values.Branch, Values.Operations.ToArray());
        }
    }
}