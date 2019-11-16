using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Netezos.Forge.Operations
{
    public class Script
    {
        [JsonProperty("code")]
        public JToken Code { get; set; }

        [JsonProperty("storage")]
        public JToken Storage { get; set; }

/*        public Script(JToken code, JToken storage, ScriptMode mode = ScriptMode.Micheline)
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
                    Code = JArray.Parse(code);
                    Storage = JObject.Parse(storage);
                    break;
                default:
                    throw new NotImplementedException($"{mode} parameters mode is not implemented");
            }
        }*/
    }

    public enum ScriptMode
    {
        Micheline
    }
}
