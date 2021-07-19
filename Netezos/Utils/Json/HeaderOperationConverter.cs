using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Forging.Models;

namespace Netezos.Utils.Json
{
    public class HeaderOperationConverter : JsonConverter<HeaderOperationContent>
    {
        public override HeaderOperationContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string protocol = null;
            Operation operation;
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                reader.Read();
                var operationHash = reader.GetString();
                reader.Read();
                var sideReader = reader;
                operation = JsonSerializer.Deserialize<Operation>(ref reader, options);

                sideReader.Read();
                sideReader.Read();

                protocol = sideReader.GetString();
                
                if (operation != null) 
                    operation.Hash = operationHash;
                reader.Read();
            }
            else
            {
                operation =  JsonSerializer.Deserialize<Operation>(ref reader, options);
            }

            return new HeaderOperationContent()
            {
                Protocol = protocol,
                Branch = operation.Branch,
                Contents = operation.Contents,
                Signature = operation.Signature,
                Hash = operation.Hash,
                // Protocol = operation.
            };;
        }

        public override void Write(Utf8JsonWriter writer, HeaderOperationContent value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}