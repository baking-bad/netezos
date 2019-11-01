using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class ActivationContent : OperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "activate_account";

        [JsonProperty("pkh")]
        public string Address { get; set; }

        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
