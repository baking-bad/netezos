using System;
using System.Collections.Generic;
using Netezos.Rpc;
using Netezos.Sandbox.Models;

namespace Netezos.Sandbox
{
    internal class ConstantsBuilder : IConstantsBuilder
    {
        private SandboxConstants Constants;

        public ConstantsBuilder()
        {
            Constants = new SandboxConstants();
        }
        
        public IConstantsBuilder AddGasReserve(int gasReserve)
        {
            Constants.GasReserve = gasReserve;
            return this;
        }

        public IConstantsBuilder AddBurnReserve(int burnReserve)
        {
            Constants.BurnReserve = burnReserve;
            return this;
        }

        public IConstantsBuilder AddCounter(int counter)
        {
            Constants.Counter = counter;
            return this;
        }

        public IConstantsBuilder AddTtl(int ttl)
        {
            Constants.Ttl = ttl;
            return this;
        }

        public IConstantsBuilder AddFee(int fee)
        {
            Constants.Fee = fee;
            return this;
        }

        public IConstantsBuilder AddGasLimit(int gasLimit)
        {
            Constants.GasLimit = gasLimit;
            return this;
        }

        public IConstantsBuilder AddStorageLimit(int storageLimit)
        {
            Constants.StorageLimit = storageLimit;
            return this;
        }

        public IConstantsBuilder AddBranchOffset(int branchOffset)
        {
            Constants.BranchOffset = branchOffset;
            return this;
        }

        public SandboxConstants Build()
        {
            return Constants;
        }
    }
}