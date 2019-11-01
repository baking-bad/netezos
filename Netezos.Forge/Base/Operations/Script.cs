using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Netezos.Forge.Operations
{
    public class Script
    {
        [JsonProperty("code")]
        public object Code { get; }

        [JsonProperty("storage")]
        public object Storage { get; }

        public Script(object code, object storage, ScriptMode mode = ScriptMode.Micheline)
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
                    Code = JsonConvert.DeserializeObject(code);
                    Storage = JsonConvert.DeserializeObject(storage);
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
