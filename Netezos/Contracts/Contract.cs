using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public class Contract
    {
        public Dictionary<string, Schema> Entrypoints { get; }
        public ParameterSchema Parameter { get; }
        public StorageSchema Storage { get; }

        public Contract(IMicheline micheline)
        {
            if (!(micheline is MichelineArray array) || array.Count != 3)
                throw new FormatException("Invalid micheline");

            var parameter = array.FirstOrDefault(x => (x as MichelinePrim)?.Prim == PrimType.parameter) as MichelinePrim
                ?? throw new FormatException("Invalid micheline parameters");

            var storage = array.FirstOrDefault(x => (x as MichelinePrim)?.Prim == PrimType.storage) as MichelinePrim
                ?? throw new FormatException("Invalid micheline storage");

            Parameter = new ParameterSchema(parameter);
            Storage = new StorageSchema(storage);

            Entrypoints = new Dictionary<string, Schema> { { "default", Parameter.Schema } };

            if (Parameter.Field?.Length > 0)
                Entrypoints[Parameter.Field] = Parameter.Schema;

            ExtractEntrypoints(Parameter.Schema);
        }

        public (string, IMicheline) NormalizeParameters(string entrypoint, IMicheline value)
        {
            if (!Entrypoints.TryGetValue(entrypoint, out var schema))
                throw new ArgumentException("Entrypoint doesn't exist");

            var resultEntypoint = entrypoint;
            var resultValue = value;

            while (schema is OrSchema or
                && value is MichelinePrim prim && (prim.Prim == PrimType.Left || prim.Prim == PrimType.Right))
            {
                schema = prim.Prim == PrimType.Left ? or.Left : or.Right;
                value = prim.Args[0];

                if (schema.Field?.Length > 0)
                {
                    resultEntypoint = schema.Field;
                    resultValue = value;
                }
            }

            return (resultEntypoint, resultValue);
        }

        public string HumanizeParameters(string entrypoint, IMicheline value, JsonWriterOptions options = default)
        {
            var (normEntrypoint, normValue) = NormalizeParameters(entrypoint, value);
            return Entrypoints[normEntrypoint].Humanize(normValue, options);
        }

        public string HumanizeStorage(IMicheline value, JsonWriterOptions options = default)
        {
            return Storage.Humanize(value, options);
        }

        void ExtractEntrypoints(Schema schema)
        {
            if (schema.Field?.Length > 0)
                Entrypoints[schema.Field] = schema;

            if (schema is OrSchema or)
            {
                ExtractEntrypoints(or.Left);
                ExtractEntrypoints(or.Right);
            }
        }
    }
}
