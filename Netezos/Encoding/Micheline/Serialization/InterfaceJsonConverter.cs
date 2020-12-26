using System;
using System.Text.Json.Serialization;

namespace Netezos.Encoding.Serialization
{
    /// <summary>
    /// This is a workaround for https://github.com/dotnet/runtime/issues/33112
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public class InterfaceJsonConverterAttribute : JsonConverterAttribute
    {
        public InterfaceJsonConverterAttribute(Type converterType) : base(converterType) { }
    }
}
