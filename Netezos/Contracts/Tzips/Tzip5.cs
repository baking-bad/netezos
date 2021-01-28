namespace Netezos.Contracts
{
    public static class Tzip5
    {
        public static bool IsFA1(this ContractScript contract)
        {
            return contract.Entrypoints.TryGetValue("transfer", out var transfer) && CheckTransfer(transfer)
                && contract.Entrypoints.TryGetValue("getBalance", out var getBalance) && CheckGetBalance(getBalance)
                && contract.Entrypoints.TryGetValue("getTotalSupply", out var getTotalSupply) && CheckGetTotalSupply(getTotalSupply);
        }

        static bool CheckTransfer(Schema schema)
        {
            return schema is PairSchema pair1
                && pair1.Left is AddressSchema
                && pair1.Right is PairSchema pair2
                    && pair2.Left is AddressSchema
                    && pair2.Right is NatSchema;
        }

        static bool CheckGetBalance(Schema schema)
        {
            return schema is PairSchema pair1
                && pair1.Left is AddressSchema
                && pair1.Right is ContractSchema contract
                    && contract.Parameters is NatSchema;
        }

        static bool CheckGetTotalSupply(Schema schema)
        {
            return schema is PairSchema pair1
                && pair1.Left is UnitSchema
                && pair1.Right is ContractSchema contract
                    && contract.Parameters is NatSchema;
        }
    }
}
