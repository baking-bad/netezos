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
                var contract = new Contract(Micheline.FromJson((string)script.code));

                foreach (var sample in DJson.Read($@"..\..\..\Contracts\Parameters\{address}.json"))
                {
                    var rawEntrypoint = sample.raw.entrypoint;
                    var rawValue = Micheline.FromJson(sample.raw.value);

                    var humanized = contract.HumanizeParameters(rawEntrypoint, rawValue);

                    Assert.Equal(
                        ((string)sample.human.value).Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace(" ", ""),
                        ((string)humanized).Replace("\t", "").Replace("\r", "").Replace("\n", "").Replace(" ", "").Trim('\"'));
                }
            }
        }
    }
}
