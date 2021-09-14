using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Netezos.Forging.Models
{
    class MempoolConverter : JsonConverter<MempoolOperations>
    {
        public override MempoolOperations Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sideReader = reader;
            sideReader.Read();
            var mempoolOperation = new MempoolOperations();
            while (sideReader.HasValueSequence)
            {
                var kind = sideReader.GetString();
                switch (kind)
                {
                    case "applied":
                    {
                        mempoolOperation.Applied = JsonSerializer.Deserialize<List<Operation>>(ref reader, options);
                        break;
                    }
                    case "refused": 
                    {
                        mempoolOperation.Refused = JsonSerializer.Deserialize<List<Operation>>(ref reader, options);
                        break;
                    }
                    case "branch_refused":
                    {
                        mempoolOperation.BranchRefused = JsonSerializer.Deserialize<List<Operation>>(ref reader, options);
                        break;
                    }
                    case "branch_delayed":
                    {
                        mempoolOperation.BranchDelayed = JsonSerializer.Deserialize<List<Operation>>(ref reader, options);
                        break;
                    }
                    case "unprocessed":
                    {
                        mempoolOperation.Unprocessed = JsonSerializer.Deserialize<List<Operation>>(ref reader, options);
                        break;
                    }
                    default: throw new JsonException("Invalid mempool operations kind");
                }
            }

            return mempoolOperation;
        }

        public override void Write(Utf8JsonWriter writer, MempoolOperations value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}