using System;
using System.Diagnostics;
using System.Linq;
using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        internal static byte[] ForgeShellHeader(ShellHeaderContent header) => 
            Concat(
                ForgeInt32(header.Level, 4),
                ForgeInt32(header.Proto, 1),
                Base58.Parse(header.Predecessor, 2),
                ForgeInt64(header.Timestamp.ToUnixTime(), 8),
                ForgeInt32(header.ValidationPass, 1),
                Base58.Parse(header.OperationsHash, 3),
                ForgeFitness(header.Fitness),
                Base58.Parse(header.Context, 2)
                );

        internal static byte[] ForgeHeaderValues(ShellHeaderContent header, ProtocolDataContent protocolData) => 
            Concat(
                ForgeShellHeader(header),
                ForgeProtocolData(protocolData)
            );

        internal static byte[] ForgeBinaryPayload(ShellHeaderContent header, ProtocolDataContent protocolData, string signature) => 
            Concat(
                ForgeShellHeader(header),
                ForgeProtocolData(protocolData),
                Base58.Parse(signature ?? throw new NullReferenceException("Forge binary payload: required signature is not null"), 5)
                );

        static byte[] ForgeProtocolData(ProtocolDataContent protocolData)
        {
            if (protocolData is ActivationProtocolDataContent data)
            {
                return ForgeContent(data.Content);
            }

            return Concat(
                ForgeInt32(protocolData.Priority, 2),
                Hex.Parse(protocolData.ProofOfWorkNonce),
                protocolData.SeedNonceHash != null 
                    ? new byte[] {255}.Concat(Base58.Parse(protocolData.SeedNonceHash)) 
                    : new byte[] {0}
                );
        }

        static byte[] ForgeContent(ActivationCommandContent content) =>
            Concat(
                ForgeCommand(content.Command),
                Base58.Parse(content.Hash, 2),
                ForgeFitness(content.Fitness),
                Hex.Parse(content.ProtocolParameters)
                );

        static byte[] ForgeFitness(FitnessContent fitness) =>
            ForgeArray(fitness.ToList().Select(x => ForgeArray(Hex.Parse(x))).SelectMany(x => x).ToArray());


        static byte[] ForgeCommand(string command)
        {
            switch (command)
            {
                case "activate":
                    return new byte[] {0};
                default:
                    throw new ArgumentException($"Invalid command name into content block {command}");
            }
        }
    }
}