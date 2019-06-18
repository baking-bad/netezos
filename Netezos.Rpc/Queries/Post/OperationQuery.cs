using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Netezos.Rpc.Queries.Post
{
    public class OperationQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the Endorsement
        /// </summary>
        public EndorsementQuery Endorsement => new EndorsementQuery(this);
        /// <summary>
        /// Gets the query to the seed nonce revelation
        /// </summary>
        public SeedNonceRevelationQuery SeedNonceRevelation => new SeedNonceRevelationQuery(this);
        /// <summary>
        /// Gets the query to the 
        /// </summary>
        public DoubleEndorsementEvidenceQuery DoubleEndorsementEvidence => new DoubleEndorsementEvidenceQuery(this);
        /// <summary>
        /// Gets the query to the double endrsement evidence
        /// </summary>
        public DoubleBakingEvidenceQuery DoubleBakingEvidence => new DoubleBakingEvidenceQuery(this);
        /// <summary>
        /// Gets the query to the account activation
        /// </summary>
        public ActivateAccountQuery ActivateAccount => new ActivateAccountQuery(this);
        /// <summary>
        /// Gets the query to the proposals
        /// </summary>
        public ProposalsQuery Proposals => new ProposalsQuery(this);
        /// <summary>
        /// Gets the query to the ballot
        /// </summary>
        public BallotQuery Ballot => new BallotQuery(this);
        /// <summary>
        /// Gets the query to the reveal
        /// </summary>
        public RevealQuery Reveal => new RevealQuery(this);
        /// <summary>
        /// Gets the query to the transaction
        /// </summary>
        public TransactionQuery Transaction => new TransactionQuery(this);
        /// <summary>
        /// Gets the query to the origination
        /// </summary>
        public OriginationQuery Origination => new OriginationQuery(this);
        /// <summary>
        /// Gets the query to the delegation
        /// </summary>
        public DelegationQuery Delegation => new DelegationQuery(this);
        
        
        internal OperationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append){}
    }
}