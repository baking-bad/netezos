using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class RevealContent : ManagerOperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "reveal";

        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
    }
}
