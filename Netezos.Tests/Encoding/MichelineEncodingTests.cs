using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Encodings.Web;
using Xunit;
using Netezos.Encoding;

namespace Netezos.Tests.Encoding
{
    public class MichelineEncodingTests
    {
        [Fact]
        public void TestSamples()
        {
            var path = @"../../../Encoding/Samples";
            var count = Directory.GetFiles(path).Length / 2;
            for (int i = 0; i < count; i++)
            {
                var json = File.ReadAllText($"{path}/Sample{i}.json");
                var bytes = Base64.Parse(File.ReadAllText($"{path}/Sample{i}.base64"));

                var micheline = Micheline.FromJson(json);

                Assert.True(Equal(micheline, Micheline.FromBytes(bytes)));
                Assert.Equal(json.Length, micheline.ToJson().Length);
                Assert.Equal(bytes, micheline.ToBytes());
                Assert.Equal(json, Micheline.ToJson(bytes));
            }
        }

        [Fact]
        public void TestDeep1()
        {
            static string CreateDeepOption(int depth)
            {
                if (depth == 0) return @"{""prim"":""int""}";
                return $@"{{""prim"":""option"",""args"":[{CreateDeepOption(depth - 1)}]}}";
            }

            var json1 = CreateDeepOption(15_000);
            var m1 = Micheline.FromJson(json1);
            var b1 = m1.ToBytes();
            Assert.NotNull(m1);
            Assert.NotNull(b1);
            Assert.Equal(json1.Length, m1.ToJson().Length);
            Assert.Equal(json1, Micheline.ToJson(b1));
            Assert.True(Equal(m1, Micheline.FromBytes(b1)));
        }

        [Fact]
        public void TestDeep2()
        {
            static string CreateDeepPair(int depth)
            {
                if (depth == 0) return $@"{{""prim"":""pair"",""args"":[{{""prim"":""nat""}},{{""prim"":""unit""}}],""annots"":[""%d0""]}}";
                return $@"{{""prim"":""pair"",""args"":[{{""prim"":""unit""}},{CreateDeepPair(depth - 1)}],""annots"":[""%d{depth}""]}}";
            }

            var json2 = CreateDeepPair(15_000);
            var m2 = Micheline.FromJson(json2);
            var b2 = m2.ToBytes();
            Assert.NotNull(m2);
            Assert.NotNull(b2);
            Assert.Equal(json2.Length, m2.ToJson().Length);
            Assert.Equal(json2, Micheline.ToJson(b2));
            Assert.True(Equal(m2, Micheline.FromBytes(b2)));
        }

        [Fact]
        public void TestAnnots()
        {
            var json = @"{""prim"":""unit"",""annots"":[""%field"","":type"",""@var"",""$some"",""\""unsafe\"""",""""]}";
            var m = Micheline.FromJson(json) as MichelinePrim;

            Assert.NotNull(m);
            Assert.Equal(6, m.Annots.Count);
            Assert.True(m.Annots[0] is FieldAnnotation);
            Assert.True(m.Annots[1] is TypeAnnotation);
            Assert.True(m.Annots[2] is VariableAnnotation);
            Assert.True(m.Annots[3] is UnsafeAnnotation);
            Assert.True(m.Annots[4] is UnsafeAnnotation);
            Assert.True(m.Annots[5] is UnsafeAnnotation);
            Assert.Equal(json, m.ToJson(new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
            Assert.Equal(json, Micheline.ToJson(m.ToBytes(), new JsonWriterOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));
        }

        static bool Equal(IMicheline m1, IMicheline m2)
        {
            var stack1 = new Stack<IMicheline>(256);
            var stack2 = new Stack<IMicheline>(256);

            stack1.Push(m1);
            stack2.Push(m2);

            while (stack1.Count > 0 || stack2.Count > 0)
            {
                var e1 = stack1.Pop();
                var e2 = stack2.Pop();

                switch (e1)
                {
                    case MichelinePrim p1:
                        if (e2 is not MichelinePrim p2 || p1.Prim != p2.Prim)
                            return false;
                        if ((p1.Annots == null) != (p2.Annots == null))
                            return false;
                        if (p1.Annots != null &&
                            !p1.Annots.Select(x => x.ToString()).SequenceEqual(p2.Annots.Select(x => x.ToString())))
                            return false;
                        if ((p1.Args == null) != (p2.Args == null))
                            return false;
                        if (p1.Args != null)
                        {
                            for (int i = p1.Args.Count - 1; i >= 0; i--)
                                stack1.Push(p1.Args[i]);
                            for (int i = p2.Args.Count - 1; i >= 0; i--)
                                stack2.Push(p2.Args[i]);
                        }
                        break;
                    case MichelineArray a1:
                        if (e2 is not MichelineArray a2 || a1.Count != a2.Count)
                            return false;
                        for (int i = a1.Count - 1; i >= 0; i--)
                            stack1.Push(a1[i]);
                        for (int i = a2.Count - 1; i >= 0; i--)
                            stack2.Push(a2[i]);
                        break;
                    case MichelineInt i1:
                        if (e2 is not MichelineInt i2 || i1.Value.CompareTo(i2.Value) != 0)
                            return false;
                        break;
                    case MichelineString s1:
                        if (e2 is not MichelineString s2 || s1.Value != s2.Value)
                            return false;
                        break;
                    case MichelineBytes b1:
                        if (e2 is not MichelineBytes b2 || !b1.Value.SequenceEqual(b2.Value))
                            return false;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return true;
        }
    }
}
