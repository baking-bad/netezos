﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Netezos.Forging.Models;
using Netezos.Keys;
using Netezos.Rpc;
using Org.BouncyCastle.Math;

namespace Netezos.Forging.Sandbox.Base
{
    public class FillOperation : HeaderOperation
    {
        public SignOperation Sign => new SignOperation(Rpc, Values, Apply);
        
        internal FillOperation(TezosRpc rpc, RequiredValues requiredValues, Func<RequiredValues, Task<(ShellHeaderContent, BlockHeaderContent, Signature)>> function) 
            : base(rpc, requiredValues, function)
        {
        }

        public override Task<dynamic> ApplyAsync()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Fill missing fields essential for preapply 
        /// </summary>
        public async Task<(ShellHeaderContent, BlockHeaderContent, Signature)> Apply(RequiredValues data)
        {
            var (_, header, _) = await Function(data);

            var predShellHeader = await Rpc.Blocks[data.BlockId].Header.Shell.GetAsync<ShellHeaderContent>();
            var timestamp = predShellHeader.Timestamp;

            var protocols = await Rpc.Blocks[data.BlockId].Protocols.GetAsync<IDictionary<string, string>>();
            header.ProtocolData.ProtocolHash = protocols["next_protocol"];

            var level = predShellHeader.Level + 1;

            header.ProtocolData.Content.ProtocolParameters =
                "000005f2f205000004626f6f7473747261705f6163636f756e747300cc01000004300058000000023000370000006564706b75426b6e5732386e5737324b4736526f48745957377031325436474b63376e4162775958356d385764397344564339796176000231000e00000034303030303030303030303030000004310058000000023000370000006564706b747a4e624441556a556b36393757376759673243527542516a79507862456738644c63635959774b534b766b50766a745639000231000e00000034303030303030303030303030000004320058000000023000370000006564706b7554586b4a4447634664356e683656764d7a38706858785533426937683668716779774e466931765a5466514e6e53315256000231000e00000034303030303030303030303030000004330058000000023000370000006564706b754672526f445345624a59677852744c783270733832556461596331577766533973453131796861755a7435446743486255000231000e00000034303030303030303030303030000004340058000000023000370000006564706b76384555554836386a6d6f336637556d3550657a6d66477252463234676e664c70483373564e774a6e5635625643784c326e000231000e0000003430303030303030303030303000000004626f6f7473747261705f636f6e74726163747300050000000004636f6d6d69746d656e7473004e000000043000460000000230002600000062747a31515a4a65425554435357756b554862346968387646336635473643417652634a68000231000d000000313030353030303030303030000000017072657365727665645f6379636c657300000000000000004001626c6f636b735f7065725f6379636c6500000000000000204001626c6f636b735f7065725f636f6d6d69746d656e7400000000000000104001626c6f636b735f7065725f726f6c6c5f736e617073686f7400000000000000104001626c6f636b735f7065725f766f74696e675f706572696f640000000000000050400474696d655f6265747765656e5f626c6f636b7300170000000230000200000030000231000200000030000001656e646f72736572735f7065725f626c6f636b00000000000000404002686172645f6761735f6c696d69745f7065725f6f7065726174696f6e0008000000313034303030300002686172645f6761735f6c696d69745f7065725f626c6f636b00090000003130343030303030000270726f6f665f6f665f776f726b5f7468726573686f6c640014000000393232333337323033363835343737353830370002746f6b656e735f7065725f726f6c6c000b0000003830303030303030303000016d696368656c736f6e5f6d6178696d756d5f747970655f73697a65000000000000408f4002736565645f6e6f6e63655f726576656c6174696f6e5f746970000700000031323530303000016f726967696e6174696f6e5f73697a6500000000000010704002626c6f636b5f73656375726974795f6465706f736974000a0000003531323030303030300002656e646f7273656d656e745f73656375726974795f6465706f73697400090000003634303030303030000462616b696e675f7265776172645f7065725f656e646f7273656d656e74002200000002300008000000313235303030300002310007000000313837353030000004656e646f7273656d656e745f726577617264002200000002300008000000313235303030300002310007000000383333333333000002636f73745f7065725f6279746500040000003235300002686172645f73746f726167655f6c696d69745f7065725f6f7065726174696f6e000600000036303030300002746573745f636861696e5f6475726174696f6e000800000031393636303830000171756f72756d5f6d696e000000000000409f400171756f72756d5f6d617800000000000058bb40016d696e5f70726f706f73616c5f71756f72756d000000000000407f4001696e697469616c5f656e646f72736572730000000000000000000264656c61795f7065725f6d697373696e675f656e646f7273656d656e740002000000310000";
            var key = BigInteger.Zero.ToByteArrayUnsigned().Align(64);
            var dummySignature = new Signature(key, Prefix.sig);

            var result = await Rpc
                .Blocks
                .Head
                .Helpers
                .Preapply
                .Block
                .PostAsync<ShellHeaderWithOperations>(
                    header.ProtocolData.ProtocolHash,
                    header.ProtocolData.Content.Command,
                    header.ProtocolData.Content.Hash,
                    header.ProtocolData.Content.Fitness?.ToList(),
                    header.ProtocolData.Content.ProtocolParameters,
                    dummySignature.ToBase58(),
                    new List<List<object>>(), 
                    timestamp);

            return (result.ShellHeader, header, dummySignature);

        } 
    }
}