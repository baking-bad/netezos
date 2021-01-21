using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class KeyHashSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.key_hash;

        public KeyHashSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length != 21)
                    return Hex.Convert(micheBytes.Value);

                var prefix = micheBytes.Value[0] == 0
                    ? Prefix.tz1
                    : micheBytes.Value[0] == 1
                        ? Prefix.tz2
                        : micheBytes.Value[0] == 2
                            ? Prefix.tz3
                            : null;

                if (prefix == null)
                    return Hex.Convert(micheBytes.Value);

                var bytes = micheBytes.Value.GetBytes(1, micheBytes.Value.Length - 1);
                return Base58.Convert(bytes, prefix);
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
