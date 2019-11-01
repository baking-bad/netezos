using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public abstract class OperationContent
    {
        [JsonProperty("kind")]
        public abstract string Kind { get; }
    }
}
