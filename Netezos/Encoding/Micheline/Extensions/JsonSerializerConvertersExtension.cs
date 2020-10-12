using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    public static class JsonSerializerConvertersExtension
    {
        public static void AddMicheline(this IList<JsonConverter> converters)
        {
            converters.Add(new AnnotationConverter());
            converters.Add(new MichelineConverter());
            converters.Add(new PrimTypeConverter());
        }
    }
}
