namespace Netezos.Forging.Models
{
    public enum OperationTag
    {
        SeedNonceRevelation = 1,
        DoubleEndorsement   = 2,
        DoubleBaking        = 3,
        Activation          = 4,
        Proposals           = 5,
        Ballot              = 6,
        DoublePreendorsement= 7,
        VdfRevelation       = 8,
        FailingNoop         = 17,
        Preendorsement      = 20,
        Endorsement         = 21,

        Reveal              = 107,
        Transaction         = 108,
        Origination         = 109,
        Delegation          = 110,
        RegisterConstant    = 111,
        SetDepositsLimit    = 112,
        IncreasePaidStorage = 113,

        TxRollupOrigination         = 150,
        TxRollupSubmitBatch         = 151,
        TxRollupCommit              = 152,
        TxRollupReturnBond          = 153,
        TxRollupFinalizeCommitment  = 154,
        TxRollupRemoveCommitment    = 155,
        TxRollupRejection           = 156,
        TxRollupDispatchTickets     = 157,
        TransferTicket              = 158,
    }
}
