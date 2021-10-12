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
            if (!(script is MichelineArray array) || array.Count < 3)
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

        public IMicheline BuildParameter(string entrypoint, object value)
            => Parameter.Build(entrypoint, value);

        public IMicheline BuildParameter(string entrypoint, params object[] values)
            => Parameter.Build(entrypoint, values);

        public IMicheline BuildOptimizedParameter(string entrypoint, object value)
            => Parameter.BuildOptimized(entrypoint, value);

        public IMicheline BuildOptimizedParameter(string entrypoint, params object[] values)
            => Parameter.BuildOptimized(entrypoint, values);

        public IMicheline OptimizeParameter(string entrypoint, IMicheline value, bool immutable = true)
            => Parameter.Optimize(entrypoint, value, immutable);

        public (string, IMicheline) NormalizeParameter(string entrypoint, IMicheline value)
            => Parameter.Normalize(entrypoint, value);

        public string HumanizeParameter(string entrypoint, IMicheline value, JsonWriterOptions options = default)
            => Parameter.Humanize(entrypoint, value, options);

        public IMicheline OptimizeStorage(IMicheline value, bool immutable = true)
            => Storage.Optimize(value, immutable);

        public string HumanizeStorage(IMicheline value, JsonWriterOptions options = default)
            => Storage.Humanize(value, options);
    }
}
