using System.Globalization;
using System.Numerics;
using Xunit;
using Netezos.Contracts;
using Netezos.Encoding;

namespace Netezos.Tests.Contracts
{
    public class TestTimestampSchema
    {
        static readonly TimestampSchema Schema = new(new MichelinePrim { Prim = PrimType.timestamp });

        [Theory]
        [InlineData("0", "1970-01-01T00:00:00Z")]
        [InlineData("1704067200", "2024-01-01T00:00:00Z")]
        [InlineData("-62135596800", "0001-01-01T00:00:00Z")]
        [InlineData("253402300799", "9999-12-31T23:59:59Z")]
        [InlineData("-62135596801", "-62135596801")]
        [InlineData("253402300800", "253402300800")]
        [InlineData("99999999999999999999", "99999999999999999999")]
        public void TestFlattenInt(string seconds, string expected)
        {
            Assert.Equal(expected, Schema.Flatten(new MichelineInt(BigInteger.Parse(seconds, CultureInfo.InvariantCulture))));
        }

        // strings accepted by Script_timestamp.of_string, with the seconds they denote
        [Theory]
        // RFC 3339
        [InlineData("2024-01-01T00:00:00Z", "1704067200")]
        [InlineData("2024-01-01t00:00:00z", "1704067200")]
        [InlineData("2024-01-01 00:00:00Z", "1704067200")]
        [InlineData("2024-01-01T03:00:00+03:00", "1704067200")]
        [InlineData("2024-01-01T03:00:00+0300", "1704067200")]
        [InlineData("2024-01-01T03:00:00+03", "1704067200")]
        [InlineData("2023-12-31T21:00:00-03:00", "1704067200")]
        [InlineData("2024-01-01T00:00:00-00:00", "1704067200")]
        [InlineData("2024-01-01T00:00:00.999999999999999Z", "1704067200")]
        [InlineData("1969-12-31T23:59:59.5Z", "-1")]
        [InlineData("1970-01-01T00:00:60Z", "60")]
        [InlineData("2016-12-31T23:59:60Z", "1483228800")]
        [InlineData("2024-02-29T00:00:00Z", "1709164800")]
        [InlineData("0000-01-01T00:00:00Z", "-62167219200")]
        [InlineData("9999-12-31T23:59:59Z", "253402300799")]
        // Zarith integers
        [InlineData("1704067200", "1704067200")]
        [InlineData("+42", "42")]
        [InlineData("-42", "-42")]
        [InlineData("-+42", "-42")]
        [InlineData("0x10", "16")]
        [InlineData("-0X10", "-16")]
        [InlineData("0o17", "15")]
        [InlineData("0b1010", "10")]
        [InlineData("1_000_000", "1000000")]
        [InlineData("253402300800", "253402300800")]
        [InlineData("99999999999999999999", "99999999999999999999")]
        [InlineData("", "0")]
        [InlineData("+", "0")]
        [InlineData("-", "0")]
        [InlineData("0x", "0")]
        [InlineData("0_", "0")]
        // rejected by the protocol, parsed leniently as before
        [InlineData(" 42", "42")]
        [InlineData("2024-01-01T03:00+03:00", "1704067200")]
        public void TestParseString(string value, string seconds)
        {
            var expected = BigInteger.Parse(seconds, CultureInfo.InvariantCulture);

            var optimized = Assert.IsType<MichelineInt>(Schema.Optimize(new MichelineString(value)));
            Assert.Equal(expected, optimized.Value);

            // a readable value and its optimized form must humanize the same
            Assert.Equal(Schema.Flatten(new MichelineInt(expected)), Schema.Flatten(new MichelineString(value)));
        }

        [Theory]
        [InlineData("ABCD")]
        [InlineData("_42")]
        [InlineData("0x_ff")]
        [InlineData("++42")]
        [InlineData("2024-02-30T00:00:00Z")]
        [InlineData("2024-01-01T24:00:00Z")]
        [InlineData("2024-01-01T00:00:00+24:00")]
        [InlineData("0000-01-01T00:00:00+00:01")] // below Ptime.min
        [InlineData("9999-12-31T23:59:59-00:01")] // above Ptime.max
        [InlineData("9999-12-31T23:59:60Z")] // leap second carries over Ptime.max
        public void TestInvalidString(string value)
        {
            Assert.Throws<FormatException>(() => Schema.Optimize(new MichelineString(value)));
            Assert.Equal(value, Schema.Flatten(new MichelineString(value)));
        }

        [Fact]
        public void TestMapDateTime()
        {
            var utc = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            Assert.Equal("2024-01-01T00:00:00Z", Assert.IsType<MichelineString>(Schema.MapObject(utc, true)).Value);
            Assert.Equal("2024-01-01T00:00:00Z", Assert.IsType<MichelineString>(Schema.MapObject(utc.ToLocalTime(), true)).Value);
        }

        [Theory]
        [InlineData("th-TH")] // Buddhist calendar
        [InlineData("ar-SA")] // Umm al-Qura calendar
        [InlineData("fi-FI")] // '.' time separator, U+2212 minus sign
        public void TestCultureInvariance(string culture)
        {
            var current = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo(culture);
                Assert.Equal("2024-01-01T00:00:00Z", Schema.Flatten(new MichelineInt(1704067200)));
                Assert.Equal("-62135596801", Schema.Flatten(new MichelineInt(-62135596801)));
                Assert.Equal("2024-01-01T00:00:00Z", Schema.Flatten(new MichelineString("2024-01-01T03:00:00+03:00")));
                Assert.Equal("2024-01-01T00:00:00Z", Assert.IsType<MichelineString>(
                    Schema.MapObject(new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), true)).Value);
            }
            finally
            {
                CultureInfo.CurrentCulture = current;
            }
        }
    }
}
