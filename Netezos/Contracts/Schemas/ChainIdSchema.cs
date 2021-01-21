using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class ChainIdSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.chain_id;

        public ChainIdSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineString micheString)
            {
                return micheString.Value;
            }
            else if (value is MichelineBytes micheBytes)
            {
                if (micheBytes.Value.Length != 4)
                    return Hex.Convert(micheBytes.Value);

                return Base58.Convert(micheBytes.Value, Prefix.Net);
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
