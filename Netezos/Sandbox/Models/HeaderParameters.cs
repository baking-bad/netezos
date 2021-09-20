﻿using System.Collections.Generic;
using Netezos.Forging.Models;

namespace Netezos.Sandbox.Models
{
    /// <summary>
    /// Required values for operation blocks
    /// </summary>
    public class HeaderParameters
    {
        public string Key { get; set; }
        public Dictionary<string, string> Keys { get; set; }
        public string ProtocolHash { get; set; }
        public ProtocolParametersContent ProtocolParameters { get; set; }
        public string Signature { get; set; }
        public int MinFee { get; set; }
    }
}