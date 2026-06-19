namespace Netezos.Rpc.Queries.Post
{
    /// <summary>
    /// Rpc query to encode a DAL attestation bitset
    /// </summary>
    public class EncodeDalAttestationQuery : RpcMethod
    {
        internal EncodeDalAttestationQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }

        /// <summary>
        /// Encodes an explicit representation of attested slots per lag into a DAL attestation bitset
        /// </summary>
        /// <param name="attestations">Array of lag index and attested slot indices pairs</param>
        public Task<dynamic> PostAsync(IEnumerable<object> attestations)
            => PostAsync((object)attestations);

        /// <summary>
        /// Encodes an explicit representation of attested slots per lag into a DAL attestation bitset
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize to</typeparam>
        /// <param name="attestations">Array of lag index and attested slot indices pairs</param>
        public Task<T?> PostAsync<T>(IEnumerable<object> attestations)
            => PostAsync<T>((object)attestations);
    }
}
