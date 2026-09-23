using System.Globalization;
using System.Numerics;
using System.Text.Json;
using Netezos.Encoding;

namespace Netezos.Contracts
{
    public sealed class TimestampSchema(MichelinePrim micheline) : Schema(micheline), IFlat
    {
        const long MinSeconds = -62_135_596_800; // 0001-01-01T00:00:00Z
        const long MaxSeconds = 253_402_300_799; // 9999-12-31T23:59:59Z
        const long MinRfc3339Seconds = -62_167_219_200; // 0000-01-01T00:00:00Z, Ptime.min (Ptime.max is MaxSeconds)

        public override PrimType Prim => PrimType.timestamp;

        internal override void WriteValue(Utf8JsonWriter writer, IMicheline value)
        {
            writer.WriteStringValue(Flatten(value));
        }

        public string Flatten(IMicheline value)
        {
            if (value is MichelineInt micheInt)
                return Format(micheInt.Value);

            if (value is MichelineString micheString)
                return TryParse(micheString.Value, out var seconds) ? Format(seconds) : micheString.Value;

            throw FormatException(value);
        }

        protected override IMicheline MapValue(object? value)
        {
            return value switch
            {
                DateTime dt => new MichelineString(Format(dt.Kind == DateTimeKind.Local ? dt.ToUniversalTime() : dt)),
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
                if (TryParse(micheStr.Value, out var seconds))
                    return new MichelineInt(seconds);

                throw FormatException(value);
            }

            return value;
        }

        static string Format(BigInteger seconds)
        {
            if (seconds < MinSeconds || seconds > MaxSeconds) // DateTime overflow
                return seconds.ToString(CultureInfo.InvariantCulture);

            return Format(DateTime.UnixEpoch.AddSeconds((long)seconds));
        }

        static string Format(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        static bool TryParse(string value, out BigInteger seconds)
        {
            return TryParseRfc3339(value, out seconds)
                || TryParseZarith(value, out seconds)
                || TryParseLenient(value, out seconds);
        }

        static bool TryParseRfc3339(string s, out BigInteger seconds)
        {
            seconds = default;

            if (s.Length < 20
                || !TryParseDigits(s, 0, 4, out var year) || s[4] != '-'
                || !TryParseDigits(s, 5, 2, out var month) || s[7] != '-'
                || !TryParseDigits(s, 8, 2, out var day) || s[10] is not ('T' or 't' or ' ')
                || !TryParseDigits(s, 11, 2, out var hour) || s[13] != ':'
                || !TryParseDigits(s, 14, 2, out var minute) || s[16] != ':'
                || !TryParseDigits(s, 17, 2, out var second))
                return false;

            if (month < 1 || month > 12 || day < 1 || day > DaysInMonth(year, month) || hour > 23 || minute > 59 || second > 60) // 60s is allowed
                return false;

            var pos = 19;
            if (s[pos] == '.')
            {
                var start = ++pos;
                while (pos < s.Length && char.IsAsciiDigit(s[pos]))
                    pos++;

                if (pos == start)
                    return false;
            }

            if (pos == s.Length)
                return false;

            var offset = 0;
            if (s[pos] is '+' or '-')
            {
                var sign = s[pos] == '+' ? 1 : -1;
                if (!TryParseDigits(s, pos + 1, 2, out var hours))
                    return false;
                pos += 3;

                var minutes = 0;
                if (pos < s.Length && s[pos] == ':')
                {
                    if (!TryParseDigits(s, pos + 1, 2, out minutes))
                        return false;
                    pos += 3;
                }
                else if (pos < s.Length && char.IsAsciiDigit(s[pos]))
                {
                    if (!TryParseDigits(s, pos, 2, out minutes))
                        return false;
                    pos += 2;
                }

                if (hours > 23 || minutes > 59)
                    return false;

                offset = sign * (hours * 3600 + minutes * 60);
            }
            else if (s[pos] is 'Z' or 'z')
            {
                pos++;
            }
            else
            {
                return false;
            }

            if (pos != s.Length)
                return false;

            var total = GetDays(year, month, day) * 86_400L + hour * 3600 + minute * 60 + second - offset;
            if (total < MinRfc3339Seconds || total > MaxSeconds) // out of Ptime range
                return false;

            seconds = total;
            return true;
        }

        static bool TryParseZarith(string s, out BigInteger value)
        {
            value = default;

            var pos = 0;
            var negative = pos < s.Length && s[pos] == '-';
            if (negative)
                pos++;
            if (pos < s.Length && s[pos] == '+')
                pos++;

            var radix = 10;
            if (pos + 1 < s.Length && s[pos] == '0')
            {
                radix = s[pos + 1] switch
                {
                    'x' or 'X' => 16,
                    'o' or 'O' => 8,
                    'b' or 'B' => 2,
                    _ => 10
                };
                if (radix != 10)
                    pos += 2;
            }

            if (pos < s.Length && s[pos] == '_')
                return false;

            var res = BigInteger.Zero;
            for (; pos < s.Length; pos++)
            {
                if (s[pos] == '_')
                    continue;

                var digit = s[pos] switch
                {
                    >= '0' and <= '9' => s[pos] - '0',
                    >= 'a' and <= 'f' => s[pos] - 'a' + 10,
                    >= 'A' and <= 'F' => s[pos] - 'A' + 10,
                    _ => int.MaxValue
                };
                if (digit >= radix)
                    return false;

                res = res * radix + digit;
            }

            value = negative ? -res : res;
            return true;
        }

        static bool TryParseLenient(string s, out BigInteger seconds)
        {
            if (BigInteger.TryParse(s, out seconds))
                return true;

            if (DateTimeOffset.TryParse(s, out var datetime))
            {
                seconds = datetime.ToUnixTimeSeconds();
                return true;
            }

            return false;
        }

        static bool TryParseDigits(string s, int pos, int count, out int value)
        {
            value = 0;
            if (pos + count > s.Length)
                return false;

            for (var i = pos; i < pos + count; i++)
            {
                if (!char.IsAsciiDigit(s[i]))
                    return false;

                value = value * 10 + s[i] - '0';
            }

            return true;
        }

        static int DaysInMonth(int year, int month) => month switch
        {
            2 => year % 4 == 0 && (year % 100 != 0 || year % 400 == 0) ? 29 : 28,
            4 or 6 or 9 or 11 => 30,
            _ => 31
        };

        static long GetDays(int year, int month, int day)
        {
            var a = (14 - month) / 12;
            var y = year + 4800 - a;
            var m = month + 12 * a - 3;
            return day + (153 * m + 2) / 5 + 365L * y + y / 4 - y / 100 + y / 400 - 32_045 - 2_440_588;
        }
    }
}
