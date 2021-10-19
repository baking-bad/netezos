using Netezos.Sandbox.HeaderMethods;

namespace Netezos.Sandbox.Base
{
    public interface IBakeBlockHandler
    {
        FillMethodHandler Fill(string blockId = "head");
    }
}