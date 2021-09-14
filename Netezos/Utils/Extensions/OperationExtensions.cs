using System.Collections.Generic;
using System.Linq;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Forging.Models;

namespace Netezos
{
    static class OperationExtensions
    {
        #region functions for calculation some default fields for operation content

        /// <summary>
        /// Take hard gas limit instead of precise amount (no simulation) and calculate fee.
        /// </summary>
        /// <param name="operation">operation</param>
        public static int DefaultFee(this OperationContent operation)
        {
            return CalculateFee(operation, operation.DefaultGasLimit(), 32 + 64 + 3 * 3);
        }

        /// <summary>
        /// Calculate minimal required operation fee.
        /// </summary>
        /// <param name="operation">operation</param>
        /// <param name="consumedGas">amount of gas consumed during the simulation</param>
        /// <param name="extraSize">size of the additional operation data</param>
        /// <param name="reserve">safe reserve, just in case</param>
        /// <returns>minimal required operation fee</returns>
        private static int CalculateFee(OperationContent operation, int consumedGas, int extraSize,
            int reserve = 10)
        {
            var size = LocalForge.ForgeOperation(operation).Length + extraSize;
            var fee = SandboxParameters.MINIMAL_FEES + SandboxParameters.MINIMAL_MUTEZ_PER_BYTE * size +
                      (int)(SandboxParameters.MINIMAL_MUTEZ_PER_GAS_UNIT * consumedGas);
            return fee + reserve;
        }

        /// <summary>
        /// Get default storage limit by operation kind.
        /// </summary>
        /// <param name="operation">operation</param>
        /// <param name="constants">constants block from context</param>
        /// <returns>default storage limit</returns>
        public static int DefaultStorageLimit(this OperationContent operation, ConstantsContent constants = null)
        {
            constants = constants ?? SandboxParameters.DEFAULT_CONSTANTS;
            switch (operation)
            {
                case RevealContent _:
                    return 0;
                case DelegationContent _:
                    return 0;
                case OriginationContent _:
                    return constants.HardStorageLimitPerOperation;
                case TransactionContent _:
                    return constants.HardStorageLimitPerOperation;
                default:
                    return SandboxParameters.DEFAULT_TRANSACTION_STORAGE_LIMIT;
            }
        }

        /// <summary>
        /// Get total consumed gas for an operation group
        /// </summary>
        /// <param name="operation">operation group</param>
        /// <param name="constants">constants block from context</param>
        /// <returns>gas limit</returns>
        public static int ConsumedGas(this List<OperationContent> contents, ConstantsContent constants = null)
        {
            return 0;
        }

        /// <summary>
        /// Get default gas limit by operation kind.
        /// </summary>
        /// <param name="operation">operation</param>
        /// <param name="constants">constants block from context</param>
        /// <returns>gas limit</returns>
        public static int DefaultGasLimit(this OperationContent operation, ConstantsContent constants = null)
        {
            constants = constants ?? SandboxParameters.DEFAULT_CONSTANTS;
            switch (operation)
            {
                case RevealContent _:
                    return 1000;
                case DelegationContent _:
                    return 1000;
                case OriginationContent _:
                    return constants.HardGasLimitPerOperation;
                case TransactionContent _:
                    return constants.HardGasLimitPerOperation;
                default:
                    return SandboxParameters.DEFAULT_TRANSACTION_GAS_LIMIT;
            }
        }
        #endregion

    }
}