using System.Collections.Generic;
using Netezos.Forging.Models;
using Netezos.Keys;

namespace Netezos.Sandbox.Models
{
    /// <summary>
    /// Required values for operation blocks
    /// </summary>
    public class HeaderParameters
    {
        public Key Key { get; set; }
        public string ProtocolHash { get; set; }
        public ProtocolParametersContent ProtocolParameters { get; set; }
        public string Signature { get; set; }
        public int MinFee { get; set; }

        public HeaderParameters Copy()
        {
            return new HeaderParameters()
            {
                Key = Key,
                ProtocolHash = ProtocolHash,
                ProtocolParameters = ProtocolParameters,
                Signature = Signature,
                MinFee = MinFee
            };
        }
    }
}