using Netezos.Encoding.Serialization;

namespace Netezos.Encoding
{
    [InterfaceJsonConverter(typeof(MichelineConverter))]
    public interface IMicheline
    {
        MichelineType Type { get; }
    }
}
