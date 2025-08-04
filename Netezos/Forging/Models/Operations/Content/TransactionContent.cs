﻿using System.Text.Json.Serialization;
using Netezos.Encoding;

namespace Netezos.Forging.Models
{
    public class TransactionContent : ManagerOperationContent
    {
        [JsonPropertyName("kind")]
        public override string Kind => "transaction";

        [JsonPropertyName("amount")]
        [JsonConverter(typeof(Int64StringConverter))]
        public long Amount { get; set; }

        [JsonPropertyName("destination")]
        public required string Destination { get; set; }

        [JsonPropertyName("parameters")]
        public Parameters? Parameters { get; set; }
    }

    public class Parameters
    {
        [JsonPropertyName("entrypoint")]
        public required string Entrypoint { get; set; }

        [JsonPropertyName("value")]
        public required IMicheline Value { get; set; }
    }
}