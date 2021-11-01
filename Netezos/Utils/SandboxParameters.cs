using Netezos.Forging.Models;

namespace Netezos
{
    static class SandboxParameters
    {
        
        #region context default constants
        public static readonly int DEFAULT_BRANCH_OFFSET = 50;
        public static readonly int DEFAULT_BURN_RESERVE = 100;
        public static readonly int DEFAULT_GAS_RESERVE = 100;
        public static readonly int DEFAULT_OPERATIONS_TTL = 5;
        public static readonly int MAX_OPERATIONS_TTL = 60;

        public static int GetOperationsTtl(bool isSandbox)
        {
            return isSandbox ? MAX_OPERATIONS_TTL : DEFAULT_OPERATIONS_TTL;
        }
        #endregion

        #region Fees default constants

        public static readonly int DEFAULT_TRANSACTION_GAS_LIMIT = 1427;
        public static readonly int DEFAULT_TRANSACTION_STORAGE_LIMIT = 257;
        public static readonly int MINIMAL_FEES = 100;
        public static readonly int MINIMAL_MUTEZ_PER_BYTE = 1;
        public static readonly double MINIMAL_MUTEZ_PER_GAS_UNIT = 0.1;

        public static readonly ConstantsContent DEFAULT_CONSTANTS = new ConstantsContent()
        {
            HardGasLimitPerOperation = 1040000,
            HardStorageLimitPerOperation = 60000
        };

        #endregion
                
    }
}