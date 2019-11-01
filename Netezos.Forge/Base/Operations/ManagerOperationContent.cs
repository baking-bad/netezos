using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public abstract class ManagerOperationContent : OperationContent
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("counter")]
        public int Counter { get; set; }

        [JsonProperty("gas_limit")]
        public int GasLimit { get; set; }

        [JsonProperty("storage_limit")]
        public int StorageLimit { get; set; }
    }
}
