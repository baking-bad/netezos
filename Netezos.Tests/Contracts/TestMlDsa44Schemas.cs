using System.Text.Json;
using Xunit;
using Netezos.Contracts;
using Netezos.Encoding;

namespace Netezos.Tests.Contracts
{
    public class TestMlDsa44Schemas
    {
        const string Mdpk = "mdpk2aR8tKbMnAisEqcwuU8sRu2827NxvZipoLrjxn3kD4g8zfpqEaF5vKrq7xncHMu9nrChEcD61VX1unsmHhUXWZuHMkE3JoLPytrQQSeRArq1JUdpuMKsnXF3HAzuEVQceFW8f923d82Vre25GiX1xJRxjrWrs2ijhxWLiiCcjFSzQKEYbDwotd6Y23Eao5Nr1nmwZksZeuippx24gzze4FnceEHbzq7JQ9fppSXtzEqWbNF5M5SS3TYPY2bz2mNPJNoGuiMwrkBHwr55Lp2FzTYT3HUK3Y4PFag8SwLsEy5bciWKovccNhHyEquRhhUqs5fHY3UKftyAoSdVrK89VQpVDTtVgEWDMivgRCm1xPYkLFJgvHuzd5DitoHDDwYnPjHNBQykad6bmm8UVNiHMApaFzwRFmfwv4jf8D1wyxLCxqJ3NeFvdrKM96EK1s8Fz4SwVESMxQpWo4nasYPVUUrFgoVuBUu4rNqakHFEiT6pEahF9n6S7eSwec4dESfNCw2CCaeaLCZt6QANk36fp8Mq87VSwquUJArSFCJmtDXswaHRqHjohyaSq2FP4P229wKdBDZwftoSu5WxzFs2usYPiGEiiH15EhZCVM2y9HxPd4k2wpQFsWxMF1sAerAAeFnaDnZHP318R6BGATgYLdjKsQX9BquSf8TJ6qd772CDxoRZaiKD573rPEcTe7nTiGdeHym1FSWKCFut8H3Wvrwx5waxJRda8u4CTdpd7pb2WCCswrG3e8vsxvNuEhCiNrwyHtQGnme2ZqJX7oCCE7wurVojusW3TDoGB74vn4YA7zfT6GLpL9teuJVHNRvy5h6ik5Jkf1m8PHz42Yt6Wtr82wAHtSkNEVHrwdDjADfz6zvFcZR9mq8Wd4DiVCy7ixhSB52skxhciDAySu9DmfwmJf1u8hEaC4Juh33VJ5CdZAfpFkQctACXxKNM2UsqBfumWSt2JmSYSECjFRcN61SMmXXoSiumNdF4bPQ32aBa75X6Y1KKMEmx2nDTW3uN18qrxcKkbybPawp8CtzS8LHreedk31woG2HVpk92TMv9n2YgWduv8NfQiyT2aF1TLYnf4ks8MdmdouqRQKLNVbDai531R6244oZyeKqmzdYe8MufT8Vo6cArKdk3s4bYxAX7auRD84TdPBy3MsQWZc4QVjR9YWuMvR7AxATyVYf4gq4YqEf3Y2nL31exqNXXz44iYnnAEALLbXDWpfPBv8M1zEMLvFhjviRuecvBPykYeeW9BgxX8Q1FDmKULBqNniwxDq5zFHEyAdmUKiuN5KzKZ6GmQAWy4V1zq2ASdegf5JPzUAhKzQ9rzfdfQ72rZ8asaamqRTW1ukDKXrnwJA8nALeoMzUQYNNbgNUtrQGCthzRipKbZeALE72bxtacs4Ht8KCgSCUWe12uyaBFBGVxo9ZuGBdrruK83y1WDDLuwSgkyXRWpmQ3nT92uPkwDARK9CbeNbimTBs8rxGYaExxxfETzpzfnovfgyhq2CNUky6g5m3BaG1AUpxoqzsR8vcNhCbf4UrzA3LjLpy7wrpUhG7PfXb11kwY8REhqeDUe7nzzYBouFzVW3VNkUdMykKhv3tGTL5k94bJUMYBXkzuppYGYyeKd6uBHrzJkNbW5ZLu7v8njQVSJuFReyz7GuHgivewomeygLepTok3GqMhYo94MnUf7tgYpz6A2bdE1BD2uxEHeM3wZjDBuWLnnudJ2rS6M1u2wvqPndUWKaCv82cXTHDTFnKjhXgBA4qTjJPb9J1YbW9mtKYmMEdLHj49tc";
        const string Tz5 = "tz5Mi6EudprASvXKjn5M23HT7f5zp2RAoxS4";
        const string Tz5Hex = "07920f1da50089947345ae7de83fe47f3ded5b1d";

        [Fact]
        public void TestKeyHashSchema()
        {
            var schema = new KeyHashSchema(new MichelinePrim { Prim = PrimType.key_hash });

            var optimized = Assert.IsType<MichelineBytes>(schema.Optimize(new MichelineString(Tz5)));
            Assert.Equal("04" + Tz5Hex, Hex.Convert(optimized.Value));

            Assert.Equal(Tz5, schema.Flatten(optimized));
            Assert.Equal(Tz5, schema.Flatten(new MichelineString(Tz5)));
            Assert.Equal($"\"{Tz5}\"", schema.Humanize(optimized));
        }

        [Fact]
        public void TestAddressSchema()
        {
            var schema = new AddressSchema(new MichelinePrim { Prim = PrimType.address });

            var optimized = Assert.IsType<MichelineBytes>(schema.Optimize(new MichelineString(Tz5)));
            Assert.Equal("0004" + Tz5Hex, Hex.Convert(optimized.Value));
            Assert.Equal(Tz5, schema.Flatten(optimized));
            Assert.Equal($"\"{Tz5}\"", schema.Humanize(optimized));

            var withEntrypoint = Assert.IsType<MichelineBytes>(schema.Optimize(new MichelineString($"{Tz5}%foo")));
            Assert.Equal("0004" + Tz5Hex + Hex.Convert(Utf8.Parse("foo")), Hex.Convert(withEntrypoint.Value));
            Assert.Equal($"{Tz5}%foo", schema.Flatten(withEntrypoint));
        }

        [Fact]
        public void TestContractSchema()
        {
            var schema = new ContractSchema(new MichelinePrim { Prim = PrimType.contract, Args = [new MichelinePrim { Prim = PrimType.unit }] });

            var optimized = Assert.IsType<MichelineBytes>(schema.Optimize(new MichelineString(Tz5)));
            Assert.Equal("0004" + Tz5Hex, Hex.Convert(optimized.Value));
            Assert.Equal(Tz5, schema.Flatten(optimized));
        }

        [Fact]
        public void TestKeySchema()
        {
            var schema = new KeySchema(new MichelinePrim { Prim = PrimType.key });

            var optimized = Assert.IsType<MichelineBytes>(schema.Optimize(new MichelineString(Mdpk)));
            Assert.Equal(1313, optimized.Value.Length);
            Assert.Equal(4, optimized.Value[0]);
            Assert.Equal(Base58.Parse(Mdpk, Prefixes.mdpk), optimized.Value[1..]);

            Assert.Equal(Mdpk, schema.Flatten(optimized));
            Assert.Equal(Mdpk, schema.Flatten(new MichelineString(Mdpk)));
            Assert.Equal($"\"{Mdpk}\"", schema.Humanize(optimized));
        }

        [Fact]
        public void TestMultisigStorageWithMlDsa44Key()
        {
            var script = new ContractScript(Micheline.FromJson(@"[
                {""prim"":""parameter"",""args"":[{""prim"":""unit""}]},
                {""prim"":""storage"",""args"":[{""prim"":""pair"",""args"":[
                    {""prim"":""nat"",""annots"":[""%stored_counter""]},
                    {""prim"":""pair"",""args"":[
                        {""prim"":""nat"",""annots"":[""%threshold""]},
                        {""prim"":""list"",""args"":[{""prim"":""key""}],""annots"":[""%keys""]}]}]}]},
                {""prim"":""code"",""args"":[[]]}]")!);

            var storage = Micheline.FromJson(@"{""prim"":""Pair"",""args"":[{""int"":""0""},{""prim"":""Pair"",""args"":[{""int"":""3""},[
                {""string"":""edpkuMEEwijhZWmqFdW2gy3HGPzZg5XQwHr1k1d2BWmuGtm6caeAYp""},
                {""string"":""p2pk66v2CPkjVmdts6YaCFtnZL1GomTjmX9ChmY93MZcaQmdi6Qp6FF""},
                {""string"":""p2pk66rhSbQgconqZpJw9BuQrQraa1w48wsodZPZU4X27AhkDmg7KKp""},
                {""string"":""" + Mdpk + @"""}]]}]}")!;

            var optimized = script.OptimizeStorage(storage);
            var keys = Assert.IsType<MichelineArray>(((MichelinePrim)((MichelinePrim)optimized).Args![1]).Args![1]);
            Assert.All(keys, key => Assert.IsType<MichelineBytes>(key));
            Assert.Equal(4, ((MichelineBytes)keys[3]).Value[0]);

            foreach (var value in new[] { storage, optimized })
            {
                using var humanized = JsonDocument.Parse(script.HumanizeStorage(value));
                var humanizedKeys = humanized.RootElement.GetProperty("keys").EnumerateArray().Select(x => x.GetString()).ToList();

                Assert.Equal(4, humanizedKeys.Count);
                Assert.Equal("edpkuMEEwijhZWmqFdW2gy3HGPzZg5XQwHr1k1d2BWmuGtm6caeAYp", humanizedKeys[0]);
                Assert.Equal(Mdpk, humanizedKeys[3]);
            }
        }
    }
}
