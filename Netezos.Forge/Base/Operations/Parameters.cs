using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netezos.Forge.Operations
{
    public class Parameters
    {
        [JsonProperty("entrypoint")]
        public string Entrypoint { get; set; }

        [JsonProperty("value")]
        public JToken Value { get; set; }

/*        public Parameters(string entrypoint)
        {
            Entrypoint = entrypoint;
            Value = null;
        }

        public Parameters(string entrypoint, string value, ParametersMode mode = ParametersMode.Micheline)
        {
            Entrypoint = entrypoint;
            switch (mode)
            {
                case ParametersMode.Micheline:
                    Value = JToken.Parse(value);
                    break;
                default:
                    throw new NotImplementedException($"{mode} parameters mode is not implemented");
            }
        }

        public Parameters(string entrypoint, JToken value, ParametersMode mode = ParametersMode.Micheline)
        {
            Entrypoint = entrypoint;
            switch (mode)
            {
                case ParametersMode.Micheline:
                    Value = value;
                    break;
                default:
                    throw new NotImplementedException($"{mode} parameters mode is not implemented");
            }
        }*/
    }

    public enum ParametersMode
    {
        Micheline
    }
}
