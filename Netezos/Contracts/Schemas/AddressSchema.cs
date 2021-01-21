using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class AddressSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.address;

        public AddressSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length < 22)
                    return Hex.Convert(micheBytes.Value);

                var prefix = micheBytes.Value[0] == 0 && micheBytes.Value[1] == 0
                    ? Prefix.tz1
                    : micheBytes.Value[0] == 0 && micheBytes.Value[1] == 1
                        ? Prefix.tz2
                        : micheBytes.Value[0] == 0 && micheBytes.Value[1] == 2
                            ? Prefix.tz3
                            : micheBytes.Value[0] == 1 && micheBytes.Value[21] == 0
                                ? Prefix.KT1
                                : null;

                if (prefix == null)
                    return Hex.Convert(micheBytes.Value);

                var bytes = micheBytes.Value[0] == 0
                    ? micheBytes.Value.GetBytes(2, 20)
                    : micheBytes.Value.GetBytes(1, 20);

                var address = Base58.Convert(bytes, prefix);
                var entrypoint = micheBytes.Value.Length > 22
                    ? Utf8.Convert(micheBytes.Value.GetBytes(22, micheBytes.Value.Length - 22))
                    : string.Empty;

                return entrypoint.Length == 0 ? address : $"{address}%{entrypoint}";
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
