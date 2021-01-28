using System;
using System.IO;
using System.Linq;
using Dynamic.Json;
using Xunit;

using Netezos.Contracts;
using Netezos.Encoding;

namespace Netezos.Tests.Contracts
{
    public class TestContracts
    {
        [Fact]
        public void TestHumanizeParameters()
        {
            foreach (var address in Directory.GetFiles($@"..\..\..\Contracts\Parameters").Select(x => x.Substring(x.Length - 41, 36)))
            {
                var script = DJson.Read($@"..\..\..\Contracts\Scripts\{address}.json");
                var contract = new ContractScript(Micheline.FromJson((string)script.code));

                foreach (var sample in DJson.Read($@"..\..\..\Contracts\Parameters\{address}.json"))
                {
                    var rawEntrypoint = sample.raw.entrypoint;
                    var rawValue = Micheline.FromJson(sample.raw.value);

                    var (normEntrypoint, normValue) = contract.NormalizeParameters((string)rawEntrypoint, (IMicheline)rawValue);
                    var humanized = contract.HumanizeParameters(normEntrypoint, normValue);

                    Assert.Equal(
                        ((string)sample.human.value).Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace(" ", ""),
                        humanized.Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace(" ", "").Trim('\"'));
                }
            }
        }
    }
}
