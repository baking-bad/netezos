using System;
using System.IO;
using System.Linq;
using Dynamic.Json;
using Xunit;
using NJsonSchema;

using Netezos.Contracts;
using Netezos.Encoding;

namespace Netezos.Tests.Contracts
{
    public class TestSchema
    {
        [Fact]
        public async void TestJsonSchemaGeneration()
        {
            foreach (var address in Directory.GetFiles($@"../../../Contracts/Parameters").Select(x => x.Substring(x.Length - 41, 36)))
            {
                var script = DJson.Read($@"../../../Contracts/Scripts/{address}.json");
                var contract = new ContractScript(Micheline.FromJson((string)script.code));

                foreach (var sample in DJson.Read($@"../../../Contracts/Parameters/{address}.json"))
                {
                    var jsonSchemaSrc = contract.Parameter.GetJsonSchema((string)sample.human.entrypoint);
                    var jsonSchema = await JsonSchema.FromJsonAsync(jsonSchemaSrc);

                    if (jsonSchema.Type != JsonObjectType.Object)
                        continue;  // Newtonsoft does not handle that

                    var errors = jsonSchema.Validate((string)sample.human.value);
                    Assert.Empty(errors);
                }
            }        
        }
    }
}
