using Netezos.Sandbox.BlockMethods;

namespace Netezos.Sandbox
{
    public interface IBlockOperationsClient
    {
        FillMethodHandler Fill();

        AutoFillMethodHandler AutoFill();
    }
}