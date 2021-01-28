using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public class ContractParameter
    {
        public Dictionary<string, Schema> Entrypoints { get; }

        public Schema Default => Entrypoints["default"];

        public ContractParameter(IMicheline parameter)
        {
            if ((parameter as MichelinePrim)?.Prim != PrimType.parameter)
                throw new ArgumentException("Invalid micheline: expected prim parameter");

            var root = new ParameterSchema(parameter as MichelinePrim);
            Entrypoints = new Dictionary<string, Schema> { { "default", root.Schema } };

            if (root.Field?.Length > 0)
                Entrypoints[root.Field] = root.Schema;

            ExtractEntrypoints(root.Schema);
        }

        public string Humanize(string entrypoint, JsonWriterOptions options = default)
        {
            if (!Entrypoints.TryGetValue(entrypoint, out var schema))
                throw new ArgumentException("Entrypoint doesn't exist");

            return schema.Humanize(options);
        }

        public string Humanize(string entrypoint, IMicheline value, JsonWriterOptions options = default)
        {
            if (!Entrypoints.TryGetValue(entrypoint, out var schema))
                throw new ArgumentException("Entrypoint doesn't exist");

            return schema.Humanize(value, options);
        }

        public (string, IMicheline) Normalize(string entrypoint, IMicheline value)
        {
            if (!Entrypoints.TryGetValue(entrypoint, out var schema))
                throw new ArgumentException("Entrypoint doesn't exist");

            var resultEntypoint = entrypoint;
            var resultValue = value;

            while (schema is OrSchema or
                && value is MichelinePrim prim && (prim.Prim == PrimType.Left || prim.Prim == PrimType.Right) && prim.Args?.Count == 1)
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

        public bool IsEntrypointUseful(string entrypoint)
        {
            if (!Entrypoints.TryGetValue(entrypoint, out var schema))
                throw new ArgumentException("Entrypoint doesn't exist");

            return !(schema is OrSchema or && or.Children().All(x => x.Field?.Length > 0));
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
