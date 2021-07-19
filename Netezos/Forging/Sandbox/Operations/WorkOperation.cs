using System;
using System.Linq;
using System.Threading.Tasks;
using Netezos.Encoding;
using Netezos.Forging.Models;
using Netezos.Rpc;
using Netezos.Utils;
using Org.BouncyCastle.Math;

namespace Netezos.Forging.Sandbox.Operations
{
    public class WorkOperation : HeaderOperation
    {
        /// <summary>
        /// Perform calculations to find proof-of-work nonce
        /// </summary>
        internal WorkOperation(
            TezosRpc rpc, 
            HeaderParameters headerParameters, 
            Func<HeaderParameters, Task<ForwardingParameters>> function) 
            : base(rpc, headerParameters, function) { }

        public SignOperation Sign => new SignOperation(Rpc, Values, CallAsync);

        public override async Task<dynamic> CallAsync() => await CallAsync(Values);

        internal override async Task<ForwardingParameters> CallAsync(HeaderParameters values)
        {
            var parameters = await Function.Invoke(Values);

            int.TryParse(values.ProtocolParameters.proof_of_work_threshold.ToString(), out int threshold);

            var nonce = 1;
            while (PowStamp(parameters.ShellHeader, parameters.BlockHeader.ProtocolData) > threshold)
            {
                parameters.BlockHeader.ProtocolData.ProofOfWorkNonce = Hex.Convert(LocalForge.ForgeInt64(nonce));
                nonce++;
            }

            return parameters;
        }

        private long PowStamp(ShellHeaderContent header, ProtocolDataContent protocolData)
        {
            var data = LocalForge.Concat(
                LocalForge.ForgeHeaderValues(header, protocolData),
                BigInteger.Zero.ToByteArrayUnsigned().Align(64));

            var hashDigest = Blake2b.GetDigest(data).Take(8).ToArray();

            return BitConverter.ToInt64(hashDigest, 0);
        }
    }
}