namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to decode a DAL attestation bitset
    /// </summary>
    public class DecodeDalAttestationQuery : RpcObject
    {
        /// <summary>
        /// Decodes a DAL attestation bitset into an explicit representation of attested slots per lag
        /// </summary>
        /// <param name="dalAttestationBitset">A DAL attestation bitset as a decimal integer</param>
        public RpcObject this[long dalAttestationBitset] => new(this, $"{dalAttestationBitset}/");

        internal DecodeDalAttestationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
