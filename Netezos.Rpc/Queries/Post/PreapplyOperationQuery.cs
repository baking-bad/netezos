namespace Netezos.Rpc.Queries.Post
{
    public class PreapplyOperationQuery : RpcQuery
    {
        internal PreapplyOperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
        {
        }

        /// <summary>
        ///     Gets the query to the Endorsement
        /// </summary>
        public PreapplyEndorsementQuery Endorsement => new PreapplyEndorsementQuery(this);

        /// <summary>
        ///     Gets the query to the seed nonce revelation
        /// </summary>
        public PreapplySeedNonceRevelationQuery SeedNonceRevelation => new PreapplySeedNonceRevelationQuery(this);

        /// <summary>
        ///     Gets the query to the
        /// </summary>
        public PreapplyDoubleEndorsementEvidenceQuery DoubleEndorsementEvidence => new PreapplyDoubleEndorsementEvidenceQuery(this);

        /// <summary>
        ///     Gets the query to the double endrsement evidence
        /// </summary>
        public PreapplyDoubleBakingEvidenceQuery DoubleBakingEvidence => new PreapplyDoubleBakingEvidenceQuery(this);

        /// <summary>
        ///     Gets the query to the account activation
        /// </summary>
        public ActivateAccountQuery ActivateAccount => new ActivateAccountQuery(this);

        /// <summary>
        ///     Gets the query to the proposals
        /// </summary>
        public ProposalsQuery Proposals => new ProposalsQuery(this);

        /// <summary>
        ///     Gets the query to the ballot
        /// </summary>
        public BallotQuery Ballot => new BallotQuery(this);

        /// <summary>
        ///     Gets the query to the reveal
        /// </summary>
        public RevealQuery Reveal => new RevealQuery(this);

        /// <summary>
        ///     Gets the query to the transaction
        /// </summary>
        public TransactionQuery Transaction => new TransactionQuery(this);

        /// <summary>
        ///     Gets the query to the origination
        /// </summary>
        public OriginationQuery Origination => new OriginationQuery(this);

        /// <summary>
        ///     Gets the query to the delegation
        /// </summary>
        public DelegationQuery Delegation => new DelegationQuery(this);
    }
}