using System;
using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class TimestampSchema : Schema, IFlat
    {
        public override PrimType Prim => PrimType.timestamp;

        public TimestampSchema(MichelinePrim micheline) : base(micheline) { }

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineInt micheInt)
            {
                if (micheInt.Value > 253_402_300_800) // DateTime overflow
                    return micheInt.Value.ToString();

                return new DateTime(1970, 1, 1)
                    .AddSeconds((long)micheInt.Value)
                    .ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else if (value is MichelineString micheString)
            {
                if (BigInteger.TryParse(micheString.Value, out var seconds))
                {
                    if (seconds > 253_402_300_800) // DateTime overflow
                        return seconds.ToString();

                    return new DateTime(1970, 1, 1)
                        .AddSeconds((long)seconds)
                        .ToString("yyyy-MM-ddTHH:mm:ssZ");
                }

                if (DateTimeOffset.TryParse(micheString.Value, out var datetime))
                    return datetime.ToString("yyyy-MM-ddTHH:mm:ssZ");

                return micheString.Value;
            }
            else
            {
                throw FormatException(value);
            }
        }
    }
}
