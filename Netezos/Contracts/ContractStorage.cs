using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public class ContractStorage
    {
        public Schema Schema { get; }

        public ContractStorage(IMicheline storage)
        {
            if ((storage as MichelinePrim)?.Prim != PrimType.storage)
                throw new ArgumentException("Invalid micheline: expected prim storage");

            Schema = new StorageSchema(storage as MichelinePrim).Schema;
        }

        public string Humanize(JsonWriterOptions options = default)
        {
            return Schema.Humanize(options);
        }

        public string Humanize(IMicheline value, JsonWriterOptions options = default)
        {
            return Schema.Humanize(value, options);
        }
    }
}
