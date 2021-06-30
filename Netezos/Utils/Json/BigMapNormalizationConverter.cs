using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Netezos.Forging.Models;
using Netezos.Rpc;

namespace Netezos.Utils.Json
{
    public class BigMapNormalizationConverter : JsonConverter<BigMapNormalization>
    {
        public override BigMapNormalization Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BigMapNormalization value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}