using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Encoding;
using Org.BouncyCastle.Math;
using Netezos.Forging.Models;
using Netezos.Forging.Sandbox.Base;
using Netezos.Keys;
using Netezos.Rpc;

namespace Netezos.Forging.Sandbox
{
    public partial class SandboxBlockHeaderService
    {
        private static string Protocol => "FLORENCE";
        private static string ProtocolHash => "PsFLorenaUUuikDWvMDr6fGBRG8kt3e3D3fHoXK1j1BFRxeSH4i";
        private static string DictatorKey => "edsk31vznjHSSpGExDMHYASz45VZqXN4DPxvsa4hAyY8dHM28cZzp6";
        private HeaderClient HeaderClient;

        public SandboxBlockHeaderService(TezosRpc rpc)
        {
            HeaderClient = new HeaderClient(rpc, ProtocolHash, DictatorKey, "genesis");
        }

        public async Task ActivationTest()
        {
            await HeaderClient.ActivateProtocol.Fill.Sign.InjectBlock.ApplyAsync();
        }
        
        
        /*/// <summary>
        /// 
        /// </summary>
        /// <param name="blockId"></param>
        /// <returns></returns>
        public async Task<SandboxBlockHeaderService> ActivateProtocol(string dictatorKey = null, string blockId = "head")
        {
            var header = await Rpc.Blocks.Head.Header.Shell.GetAsync<ShellHeaderContent>();
            var fitness = header.Fitness.BumpFitness();
            return await FillBlock(command:"activate", fitness: fitness);
        }

        public async Task BakeBlock(string minFee, string blockId = "head")
        {
            
        }*/

        /// <summary>
        /// Fill missing fields essential for preapply 
        /// </summary>
        /// <param name="command">command activate</param>
        /// <param name="blockId">block Id(optional) default head</param>
        /// <param name="fitness">fitness(optional)</param>
        /// <returns></returns>
/*        private async Task<SandboxBlockHeaderService> FillBlock(string command, string blockId = "head", FitnessContent fitness = null)
        {
            var predShellHeader = await Rpc.Blocks[blockId].Header.Shell.GetAsync<ShellHeaderContent>();
            var timestamp = predShellHeader.Timestamp;

            var protocols = await Rpc.Blocks[blockId].Protocols.GetAsync<IDictionary<string, string>>();
            var protocol = protocols["next_protocol"];
            var level = predShellHeader.Level + 1;

            var key = BigInteger.Zero.ToByteArrayUnsigned().Align(64);
            var dummySignature = new Signature(key, Prefix.sig);

            var result = await Rpc
                .Blocks
                .Head
                .Helpers
                .Preapply
                .Block
                .PostAsync<ShellHeaderWithOperations>(
                    protocol,
                    command,
                    ProtocolHash,
                    fitness?.ToList(), 
                    SandboxParameters,
                    dummySignature.ToBase58(),
                    new List<List<object>>(), 
                    timestamp);

            return this;
        }

        /// <summary>
        /// Sign the block header with the specified key
        /// </summary>
        /// <returns>signature string</returns>
        public async Task<SandboxBlockHeaderService> SignBlockHeader()
        {
            var chainId = await Rpc.GetAsync<string>("chain/main/chain_id");
            var watermark = new byte[] {1}.Concat(Base58.Parse(chainId));
            var signature = Key.FromBase58(DictatorKey).Sign(watermark.Concat(LocalForge.ForgeBlockHeader(null, null)));
            return signature;
        }

        /// <summary>
        /// Inject the signed block header
        /// </summary>
        /// <returns>block hash</returns>
        public async Task<string> InjectBlockHeader(bool isForce = false)
        {
            var data = LocalForge.ForgeBinaryPayload(null, null, null);

            var hash = await Rpc.Inject.Block.PostAsync<string>(data, null, force:isForce, async:false);
            return hash;
        }*/
    }
}