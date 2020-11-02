using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class KeyHashSchema : Schema, IFlat
    {
        #region static
        static readonly byte[] Tz1Prefix = new byte[] { 6, 161, 159 };
        static readonly byte[] Tz2Prefix = new byte[] { 6, 161, 161 };
        static readonly byte[] Tz3Prefix = new byte[] { 6, 161, 164 };
        #endregion

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
                    ? Tz1Prefix
                    : micheBytes.Value[0] == 1
                        ? Tz2Prefix
                        : micheBytes.Value[0] == 2
                            ? Tz3Prefix
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
