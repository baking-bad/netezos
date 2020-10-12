using System.Text.Json;
using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    public static class JsonSerializerOptionsExtension
    {
        public static JsonSerializerOptions AddMichelineConverters(this JsonSerializerOptions options)
        {
            options.Converters.Add(new AnnotationConverter());
            options.Converters.Add(new MichelineConverter());
            options.Converters.Add(new PrimTypeConverter());

            return options;
        }
    }
}
