using System;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Sandbox.Models;
using Netezos.Utils;

namespace Netezos.Sandbox.HeaderMethods
{
    public class WorkMethodHandler : HeaderMethodHandler
    {
        /// <summary>
        /// Perform calculations to find proof-of-work nonce
        /// </summary>
        internal WorkMethodHandler(
            TezosRpc rpc, 
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<ForwardingParameters>> function) 
            : base(rpc, headerParameters, function) { }

        public SignMethodHandler Sign => new SignMethodHandler(Rpc, Values, CallAsync);

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters values)
        {
            var parameters = await Function.Invoke(Values);

            decimal.TryParse(values.ProtocolParameters.ProofOfWorkThreshold, out var threshold);

            long nonce = 0;
            while (PowStamp(parameters.BlockHeader.ShellHeader, parameters.BlockHeader.ProtocolData) > threshold)
            {
                nonce++;
                parameters.BlockHeader.ProtocolData.ProofOfWorkNonce = Hex.Convert(LocalForge.ForgeInt64(nonce));
            }

            return parameters;
        }

        private decimal PowStamp(ShellHeaderContent header, ProtocolDataContent protocolData)
        {
            var data = LocalForge.Concat(
                LocalForge.ForgeHeaderValues(header, protocolData),
                new byte[64]);

            var hashDigest = Blake2b.GetDigest(data).Take(8).ToArray();

            // byte order
            hashDigest = BitConverter.IsLittleEndian ? hashDigest.Reverse() : hashDigest;

            var pow = BitConverter.ToUInt64(hashDigest, 0);
            return pow;
        }
    }
}