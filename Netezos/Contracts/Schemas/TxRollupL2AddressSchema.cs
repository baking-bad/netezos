using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class TxRollupL2AddressSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.tx_rollup_l2_address;

        public TxRollupL2AddressSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length != 20)
                    return Hex.Convert(micheBytes.Value);

                return Base58.Convert(micheBytes.Value, Prefix.tz4);
            }
            else
            {
                throw FormatException(value);
            }
        }

        protected override IMicheline MapValue(object value)
        {
            return value switch
            {
                string str => new MichelineString(str),
                byte[] bytes => new MichelineBytes(bytes),
                JsonElement { ValueKind: JsonValueKind.String } json => new MichelineString(json.GetString()!),
                _ => throw MapFailedException("invalid value"),
            };
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelineString micheStr)
                return new MichelineBytes(Base58.Parse(micheStr.Value, 3));

            return value;
        }
    }
}
