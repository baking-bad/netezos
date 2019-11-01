using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class Parameters
    {
        [JsonProperty("entrypoint")]
        public string Entrypoint { get; }

        [JsonProperty("value")]
        public object Value { get; }

        public Parameters(string entrypoint)
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
                    Value = JsonConvert.DeserializeObject(value);
                    break;
                default:
                    throw new NotImplementedException($"{mode} parameters mode is not implemented");
            }
        }

        public Parameters(string entrypoint, object value, ParametersMode mode = ParametersMode.Micheline)
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
        }
    }

    public enum ParametersMode
    {
        Micheline
    }
}
