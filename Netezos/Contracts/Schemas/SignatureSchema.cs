using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class SignatureSchema : Schema, IFlat
    {
        #region static
        static readonly byte[] SigPrefix = new byte[] { 4, 130, 043 };
        #endregion

        public override PrimType Prim => PrimType.signature;

        public SignatureSchema(MichelinePrim micheline) : base(micheline) { }

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
                if (micheBytes.Value.Length != 64)
                    return Hex.Convert(micheBytes.Value);

                return Base58.Convert(micheBytes.Value, SigPrefix);
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
