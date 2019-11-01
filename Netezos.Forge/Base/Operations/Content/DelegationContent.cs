using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class DelegationContent : ManagerOperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "delegation";

        [JsonProperty("delegate")]
        public string Delegate { get; set; }
    }
}
