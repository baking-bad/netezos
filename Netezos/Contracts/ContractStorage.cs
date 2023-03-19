using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public class ContractStorage
    {
        public Schema Schema { get; }

        public ContractStorage(IMicheline storage)
        {
            if (storage is not MichelinePrim { Prim: PrimType.storage } prim)
                throw new ArgumentException("Invalid micheline: expected prim storage");

            Schema = new StorageSchema(prim).Schema;
        }

        public IMicheline Optimize(IMicheline value, bool immutable = true)
        {
            return Schema.Optimize(immutable ? Micheline.FromBytes(value.ToBytes()) : value);
        }

        public string Humanize(JsonWriterOptions options = default)
        {
            return Schema.Humanize(options);
        }

        public string Humanize(IMicheline value, JsonWriterOptions options = default)
        {
            return Schema.Humanize(value, options);
        }

        public string GetJsonSchema(JsonWriterOptions options = default)
        {
            return Schema.GetJsonSchema(options);
        }
    }
}
