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
                if (micheInt.Value > 253_402_300_800 || micheInt.Value < -62_135_596_800) // DateTime overflow
                    return micheInt.Value.ToString();
                
                return new DateTime(1970, 1, 1)
                    .AddSeconds((long)micheInt.Value)
                    .ToString("yyyy-MM-ddTHH:mm:ssZ");
            }
            else if (value is MichelineString micheString)
            {
                if (BigInteger.TryParse(micheString.Value, out var seconds))
                {
                    if (seconds > 253_402_300_800 || seconds < -62_135_596_800) // DateTime overflow
                        return seconds.ToString();

                    return new DateTime(1970, 1, 1)
                        .AddSeconds((long)seconds)
                        .ToString("yyyy-MM-ddTHH:mm:ssZ");
                }

                if (DateTimeOffset.TryParse(micheString.Value, out var datetime))
                    return datetime.ToString("yyyy-MM-ddTHH:mm:ssZ");

                if (micheString.Value.Length == 0)
                    return new DateTime(1970, 1, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                return micheString.Value;
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
                DateTime dt => new MichelineString(dt.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                int i => new MichelineInt(i),
                long l => new MichelineInt(l),
                string s => new MichelineString(s),
                JsonElement { ValueKind: JsonValueKind.Number } json => new MichelineInt(new BigInteger(json.GetInt64())),
                JsonElement { ValueKind: JsonValueKind.String } json => new MichelineString(json.GetString()!),
                _ => throw MapFailedException("invalid value")
            };
        }

        public override IMicheline Optimize(IMicheline value)
        {
            if (value is MichelineString micheStr)
            {
                if (BigInteger.TryParse(micheStr.Value, out var timestamp))
                    return new MichelineInt(timestamp);

                if (DateTimeOffset.TryParse(micheStr.Value, out var datetime))
                {
                    var seconds = (long)(datetime - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds;
                    return new MichelineInt(new BigInteger(seconds));
                }

                if (micheStr.Value?.Length == 0)
                    return new MichelineInt(0);

                throw FormatException(value);
            }

            return value;
        }
    }
}
