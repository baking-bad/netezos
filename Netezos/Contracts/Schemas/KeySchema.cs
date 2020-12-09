using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class KeySchema : Schema, IFlat
    {
        #region static
        static readonly byte[] EdpkPrefix = new byte[] { 13, 15, 37, 217 };
        static readonly byte[] SppkPrefix = new byte[] { 3, 254, 226, 86 };
        static readonly byte[] P2pkPrefix = new byte[] { 3, 178, 139, 127 };
        #endregion

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
                    ? EdpkPrefix
                    : micheBytes.Value[0] == 1 && micheBytes.Value.Length == 34
                        ? SppkPrefix
                        : micheBytes.Value[0] == 2 && micheBytes.Value.Length == 34
                            ? P2pkPrefix
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
