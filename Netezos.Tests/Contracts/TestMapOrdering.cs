using System.Numerics;
using System.Text.Json;
using Xunit;
using Netezos.Contracts;
using Netezos.Encoding;
using Netezos.Keys;

namespace Netezos.Tests.Contracts
{
    public class TestMapOrdering
    {
        const string KT1 = "KT1PWx2mnDueood7fEmfbBDKx1D9BAnnXitn";

        static Schema Create(string type) => Schema.Create((MichelinePrim)Micheline.FromJson(type)!);

        static IMicheline Map(string type, object value) => Create(type).MapObject(value, true);

        static IMicheline Map(string type, string json) => Create(type).MapObject(JsonDocument.Parse(json).RootElement, true);

        static string[] Keys(IMicheline map) => [.. ((MichelineArray)map).Select(x => x is MichelinePrim { Prim: PrimType.Elt } elt
            ? elt.Args![0].ToJson()
            : x.ToJson())];

        [Fact]
        public void TestNatKeysSortedNumerically()
        {
            var map = Map("""{"prim":"map","args":[{"prim":"nat"},{"prim":"string"}]}""",
                new Dictionary<int, string> { [10] = "c", [2] = "b", [1] = "a" });

            Assert.Equal(["""{"int":"1"}""", """{"int":"2"}""", """{"int":"10"}"""], Keys(map));
        }

        [Fact]
        public void TestIntKeysSortedWithNegatives()
        {
            var map = Map("""{"prim":"big_map","args":[{"prim":"int"},{"prim":"unit"}]}""",
                """[{"key":"5","value":{}},{"key":"-7","value":{}},{"key":"0","value":{}}]""");

            Assert.Equal(["""{"int":"-7"}""", """{"int":"0"}""", """{"int":"5"}"""], Keys(map));
        }

        [Fact]
        public void TestStringKeysSortedOrdinally()
        {
            var map = Map("""{"prim":"map","args":[{"prim":"string"},{"prim":"nat"}]}""",
                """{"b":1,"a":2,"B":3,"aa":4}""");

            Assert.Equal(["""{"string":"B"}""", """{"string":"a"}""", """{"string":"aa"}""", """{"string":"b"}"""], Keys(map));
        }

        [Fact]
        public void TestBytesKeysSortedLexicographically()
        {
            var set = Map("""{"prim":"set","args":[{"prim":"bytes"}]}""", new[] { "0102", "01", "ff", "" });

            Assert.Equal(["""{"bytes":""}""", """{"bytes":"01"}""", """{"bytes":"0102"}""", """{"bytes":"ff"}"""], Keys(set));
        }

        [Fact]
        public void TestAddressKeysSortedByBinaryForm()
        {
            var tz1 = new Key(ECKind.Ed25519).Address;
            var tz2 = new Key(ECKind.Secp256k1).Address;
            var tz3 = new Key(ECKind.NistP256).Address;

            var set = Map("""{"prim":"set","args":[{"prim":"address"}]}""",
                new[] { $"{KT1}%zzz", KT1, tz3, $"{KT1}%abc", tz2, tz1 });

            Assert.Equal([tz1, tz2, tz3, $"{KT1}%abc", KT1, $"{KT1}%zzz"],
                ((MichelineArray)set).Select(x => ((MichelineString)x).Value));
        }

        [Fact]
        public void TestDefaultEntrypointIsDuplicateOfNoEntrypoint()
        {
            Assert.Throws<FormatException>(() => Map("""{"prim":"set","args":[{"prim":"address"}]}""",
                new[] { KT1, $"{KT1}%default" }));
        }

        [Fact]
        public void TestKeyHashKeysSortedByCurve()
        {
            var tz1 = new Key(ECKind.Ed25519).PubKey.Address;
            var tz2 = new Key(ECKind.Secp256k1).PubKey.Address;
            var tz3 = new Key(ECKind.NistP256).PubKey.Address;

            var set = Map("""{"prim":"set","args":[{"prim":"key_hash"}]}""", new[] { tz3, tz1, tz2 });

            Assert.Equal([tz1, tz2, tz3], ((MichelineArray)set).Select(x => ((MichelineString)x).Value));
        }

        [Fact]
        public void TestMixedFormsAreComparedByValue()
        {
            var set = Map("""{"prim":"set","args":[{"prim":"timestamp"}]}""",
                new object[] { "1970-01-01T00:00:10Z", 5L, "1970-01-01T00:00:01Z" });

            Assert.Equal(["""{"string":"1970-01-01T00:00:01Z"}""", """{"int":"5"}""", """{"string":"1970-01-01T00:00:10Z"}"""], Keys(set));
        }

        [Fact]
        public void TestPairKeysSortedLexicographically()
        {
            var set = Map("""{"prim":"set","args":[{"prim":"pair","args":[{"prim":"nat","annots":["%a"]},{"prim":"string","annots":["%b"]}]}]}""",
                """[{"a":"2","b":"a"},{"a":"1","b":"z"},{"a":"1","b":"b"}]""");

            Assert.Equal([
                """{"prim":"Pair","args":[{"int":"1"},{"string":"b"}]}""",
                """{"prim":"Pair","args":[{"int":"1"},{"string":"z"}]}""",
                """{"prim":"Pair","args":[{"int":"2"},{"string":"a"}]}"""
            ], Keys(set));
        }

        [Fact]
        public void TestOptionKeysSortedNoneFirst()
        {
            var set = Map("""{"prim":"set","args":[{"prim":"option","args":[{"prim":"nat"}]}]}""",
                """["3",null,"1"]""");

            Assert.Equal([
                """{"prim":"None"}""",
                """{"prim":"Some","args":[{"int":"1"}]}""",
                """{"prim":"Some","args":[{"int":"3"}]}"""
            ], Keys(set));
        }

        [Fact]
        public void TestOrKeysSortedLeftFirst()
        {
            var set = Map("""{"prim":"set","args":[{"prim":"or","args":[{"prim":"nat","annots":["%l"]},{"prim":"string","annots":["%r"]}]}]}""",
                """[{"r":"a"},{"l":"7"},{"l":"2"}]""");

            Assert.Equal([
                """{"prim":"Left","args":[{"int":"2"}]}""",
                """{"prim":"Left","args":[{"int":"7"}]}""",
                """{"prim":"Right","args":[{"string":"a"}]}"""
            ], Keys(set));
        }

        [Fact]
        public void TestBoolAndMutezKeys()
        {
            var bools = Map("""{"prim":"set","args":[{"prim":"bool"}]}""", new[] { true, false });
            Assert.Equal(["""{"prim":"False"}""", """{"prim":"True"}"""], Keys(bools));

            var mutez = Map("""{"prim":"set","args":[{"prim":"mutez"}]}""", new[] { BigInteger.Pow(2, 63) - 1, BigInteger.One });
            Assert.Equal(["""{"int":"1"}""", """{"int":"9223372036854775807"}"""], Keys(mutez));
        }

        [Fact]
        public void TestDuplicateKeysThrow()
        {
            Assert.Throws<FormatException>(() => Map("""{"prim":"set","args":[{"prim":"nat"}]}""", new[] { 1, 2, 1 }));

            Assert.Throws<FormatException>(() => Map("""{"prim":"map","args":[{"prim":"timestamp"},{"prim":"unit"}]}""",
                """[{"key":"1970-01-01T00:00:01Z","value":{}},{"key":1,"value":{}}]"""));
        }

        [Fact]
        public void TestNestedMapsSorted()
        {
            var map = Map("""{"prim":"map","args":[{"prim":"string"},{"prim":"set","args":[{"prim":"nat"}]}]}""",
                """{"y":["2","1"],"x":[]}""");

            Assert.Equal("""[{"prim":"Elt","args":[{"string":"x"},[]]},{"prim":"Elt","args":[{"string":"y"},[{"int":"1"},{"int":"2"}]]}]""",
                map.ToJson());
        }
    }
}
