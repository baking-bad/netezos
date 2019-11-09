using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class TransactionContent : ManagerOperationContent
    {
        [JsonProperty("kind")]
        public override string Kind => "transaction";

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        [JsonProperty("parameters")]
        public Parameters Parameters { get; set; }

        public static TransactionContent AutoFill(long amount, string destination, Parameters parameters = null) =>
            throw new NotImplementedException();
    }
}