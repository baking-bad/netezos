namespace Netezos.Forging.Models
{
    public enum OperationTag
    {
        Endorsement         = 0,
        SeedNonceRevelation = 1,
        DoubleEndorsement   = 2,
        DoubleBaking        = 3,
        Activation          = 4,
        Proposals           = 5,
        Ballot              = 6,
        Reveal              = 107,
        Transaction         = 108,
        Origination         = 109,
        Delegation          = 110
    }
}
