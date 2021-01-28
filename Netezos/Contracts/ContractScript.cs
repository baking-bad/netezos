using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public class ContractScript
    {
        public ContractParameter Parameter { get; }
        public ContractStorage Storage { get; }

        public Dictionary<string, Schema> Entrypoints => Parameter.Entrypoints;

        public ContractScript(IMicheline script)
        {
            if (!(script is MichelineArray array) || array.Count != 3)
                throw new FormatException("Invalid micheline");

            var parameter = array.FirstOrDefault(x => (x as MichelinePrim)?.Prim == PrimType.parameter) as MichelinePrim
                ?? throw new FormatException("Invalid micheline parameters");

            var storage = array.FirstOrDefault(x => (x as MichelinePrim)?.Prim == PrimType.storage) as MichelinePrim
                ?? throw new FormatException("Invalid micheline storage");

            Parameter = new ContractParameter(parameter);
            Storage = new ContractStorage(storage);
        }

        public ContractScript(IMicheline parameter, IMicheline storage)
        {
            Parameter = new ContractParameter(parameter);
            Storage = new ContractStorage(storage);
        }

        public (string, IMicheline) NormalizeParameters(string entrypoint, IMicheline value)
            => Parameter.Normalize(entrypoint, value);

        public string HumanizeParameters(string entrypoint, IMicheline value, JsonWriterOptions options = default)
            => Parameter.Humanize(entrypoint, value, options);

        public string HumanizeStorage(IMicheline value, JsonWriterOptions options = default)
            => Storage.Humanize(value, options);
    }
}
