using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class OriginationContent : ManagerOperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "origination";

        [JsonProperty("balance")]
        public long Balance { get; set; }

        [JsonProperty("delegate")]
        public string Delegate { get; set; }

        [JsonProperty("script")]
        public Script Script { get; set; }
    }
}
