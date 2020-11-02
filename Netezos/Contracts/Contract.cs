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
        public Schema Storage { get; }
        public Tzip Standards { get; }

        public Contract(IMicheline micheline)
        {
            if (!(micheline is MichelineArray array) || array.Count != 3)
                throw new FormatException("Invalid micheline");

            #region entrypoints
            var parameters = array.FirstOrDefault(x => (x as MichelinePrim)?.Prim == PrimType.parameter) as MichelinePrim
                ?? throw new FormatException("Invalid micheline parameters");

            var defaultSchema = Schema.Create(parameters.Args[0] as MichelinePrim);
            Entrypoints = new Dictionary<string, Schema> { { "default", defaultSchema } };

            var annot = parameters.Annots?.FirstOrDefault(x => x.Type == AnnotationType.Field);
            if (annot != null && annot.Value.Length > 0)
                Entrypoints[annot.Value] = defaultSchema;

            ExtractEntrypoints(defaultSchema);
            #endregion

            #region storage
            var storage = array.FirstOrDefault(x => (x as MichelinePrim)?.Prim == PrimType.storage) as MichelinePrim
                ?? throw new FormatException("Invalid micheline storage");

            Storage = Schema.Create(storage.Args[0] as MichelinePrim);
            #endregion

            //TODO: check for implemented standards
            Standards = Tzip.None;
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
