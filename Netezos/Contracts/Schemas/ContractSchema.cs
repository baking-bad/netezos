using System;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class ContractSchema : Schema, IFlat
    {
        #region static
        static readonly byte[] Tz1Prefix = new byte[] { 6, 161, 159 };
        static readonly byte[] Tz2Prefix = new byte[] { 6, 161, 161 };
        static readonly byte[] Tz3Prefix = new byte[] { 6, 161, 164 };
        static readonly byte[] KT1Prefix = new byte[] { 2, 90, 121 };
        #endregion

        public override PrimType Prim => PrimType.contract;

        public Schema Parameters { get; }

        public ContractSchema(MichelinePrim micheline) : base(micheline)
        {
            if (micheline.Args?.Count != 1 || !(micheline.Args[0] is MichelinePrim type))
                throw new FormatException($"Invalid {Prim} schema format");

            Parameters = Create(type);
        }

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
                    ? Tz1Prefix
                    : micheBytes.Value[0] == 0 && micheBytes.Value[1] == 1
                        ? Tz2Prefix
                        : micheBytes.Value[0] == 0 && micheBytes.Value[1] == 2
                            ? Tz3Prefix
                            : micheBytes.Value[0] == 1 && micheBytes.Value[21] == 0
                                ? KT1Prefix
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
