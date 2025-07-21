namespace Netezos.Forging.Models
{
    public enum OperationTag
    {
        SeedNonceRevelation = 1,
        DoubleConsensus     = 2,
        DoubleBaking        = 3,
        Activation          = 4,
        Proposals           = 5,
        Ballot              = 6,
        VdfRevelation       = 8,
        DrainDelegate       = 9,

        FailingNoop = 17,
        
        Preattestation          = 20,
        Attestation             = 21,
        AttestationWithDal      = 23,
        DalEntrapmentEvidence   = 24,

        PreattestationsAggregate = 30,
        AttestationsAggregate    = 31,

        Reveal              = 107,
        Transaction         = 108,
        Origination         = 109,
        Delegation          = 110,
        RegisterConstant    = 111,
        SetDepositsLimit    = 112,
        IncreasePaidStorage = 113,
        UpdateConsensusKey  = 114,
        UpdateCompanionKey  = 115,

        TransferTicket = 158,
        
        SrOriginate   = 200,
        SrAddMessages = 201,
        SrCement      = 202,
        SrPublish     = 203,
        SrRefute      = 204,
        SrTimeout     = 205,
        SrExecute     = 206,
        SrRecoverBond = 207,

        DalPublishCommitment = 230
    }
}
