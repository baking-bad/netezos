using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    internal interface IConstantsBuilder
    {
        IConstantsBuilder AddGasReserve(int gasReserve);
        IConstantsBuilder AddBurnReserve(int burnReserve);
        IConstantsBuilder AddCounter(int counter);
        IConstantsBuilder AddTtl(int ttl);
        IConstantsBuilder AddFee(int fee);
        IConstantsBuilder AddGasLimit(int gasLimit);
        IConstantsBuilder AddStorageLimit(int storageLimit);
        IConstantsBuilder AddBranchOffset(int branchOffset);
        SandboxConstants Build();
    }
}