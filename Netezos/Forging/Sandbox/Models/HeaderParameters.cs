using System.Collections.Generic;

namespace Netezos.Forging.Sandbox
{
    /// <summary>
    /// Required values for operation blocks
    /// </summary>
    internal class HeaderParameters
    {
        public string Key { get; set; }
        public Dictionary<string, string> Keys { get; set; }
        public string BlockId { get; set; }
        public string ProtocolHash { get; set; }
        public dynamic ProtocolParameters { get; set; }
        public string Signature { get; set; }
        public int MinFee { get; set; } = 0;
    }
}