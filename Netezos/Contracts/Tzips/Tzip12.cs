namespace Netezos.Contracts
{
    public static class Tzip12
    {
        public static bool IsFA2(this Contract contract)
        {
            return contract.Entrypoints.TryGetValue("transfer", out var transfer) && CheckTransfer(transfer)
                && contract.Entrypoints.TryGetValue("balance_of", out var balanceOf) && CheckBalanceOf(balanceOf)
                && contract.Entrypoints.TryGetValue("update_operators", out var updateOperators) && CheckUpdateOperators(updateOperators)
                && contract.Entrypoints.TryGetValue("token_metadata_registry", out var metadataRegistry) && CheckMetadataRegistry(metadataRegistry);
        }

        static bool CheckTransfer(Schema schema)
        {
            return schema is ListSchema list1
                && list1.Item is PairSchema pair1
                    && pair1.Left is AddressSchema
                    && pair1.Right is ListSchema list2
                        && list2.Item is PairSchema pair2
                            && pair2.Left is AddressSchema
                            && pair2.Right is PairSchema pair3
                                && pair3.Left is NatSchema
                                && pair3.Right is NatSchema;
        }

        static bool CheckBalanceOf(Schema schema)
        {
            return schema is PairSchema pair1
                && pair1.Left is ListSchema list1
                    && list1.Item is PairSchema pair2
                        && pair2.Left is AddressSchema
                        && pair2.Right is NatSchema
                && pair1.Right is ContractSchema contract
                    && contract.Parameters is ListSchema list2
                    && list2.Item is PairSchema pair3
                        && pair3.Left is PairSchema pair4
                            && pair4.Left is AddressSchema
                            && pair4.Right is NatSchema
                        && pair3.Right is NatSchema;
        }

        static bool CheckUpdateOperators(Schema schema)
        {
            return schema is ListSchema list1
                && list1.Item is OrSchema or
                    && or.Left is PairSchema pair1
                        && pair1.Left is AddressSchema
                        && pair1.Right is PairSchema pair2
                            && pair2.Left is AddressSchema
                            && pair2.Right is NatSchema
                    && or.Right is PairSchema pair3
                        && pair3.Left is AddressSchema
                        && pair3.Right is PairSchema pair4
                            && pair4.Left is AddressSchema
                            && pair4.Right is NatSchema;
        }

        static bool CheckMetadataRegistry(Schema schema)
        {
            return schema is ContractSchema contract
                && contract.Parameters is AddressSchema;
        }
    }
}
