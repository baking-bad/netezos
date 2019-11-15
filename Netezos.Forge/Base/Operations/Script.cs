using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netezos.Forge.Operations
{
    public class Script
    {
        [JsonProperty("code")]
        public JToken Code { get; }

        [JsonProperty("storage")]
        public JToken Storage { get; }

        public Script(JToken code, JToken storage, ScriptMode mode = ScriptMode.Micheline)
        {
            switch (mode)
            {
                case ScriptMode.Micheline:
                    Code = code;
                    Storage = storage;
                    break;
                default:
                    throw new NotImplementedException($"{mode} parameters mode is not implemented");
            }
        }

        public Script(string code, string storage, ScriptMode mode = ScriptMode.Micheline)
        {
            switch (mode)
            {
                case ScriptMode.Micheline:
                    Code = JToken.Parse(code);
                    Storage = JToken.Parse(storage);
                    break;
                default:
                    throw new NotImplementedException($"{mode} parameters mode is not implemented");
            }
        }
    }

    public enum ScriptMode
    {
        Micheline
    }
}
