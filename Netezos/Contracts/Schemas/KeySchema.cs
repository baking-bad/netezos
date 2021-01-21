using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class KeySchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.key;

        public KeySchema(MichelinePrim micheline) : base(micheline) { }

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
                var prefix = micheBytes.Value[0] == 0 && micheBytes.Value.Length == 33
                    ? Prefix.edpk
                    : micheBytes.Value[0] == 1 && micheBytes.Value.Length == 34
                        ? Prefix.sppk
                        : micheBytes.Value[0] == 2 && micheBytes.Value.Length == 34
                            ? Prefix.p2pk
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
