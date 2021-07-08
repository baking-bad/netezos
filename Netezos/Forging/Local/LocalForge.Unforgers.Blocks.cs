using System.Linq;
using Netezos.Encoding;
using Netezos.Forging.Models;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        public static ShellHeaderContent UnforgeShellHeader(ForgedReader reader) =>
            new ShellHeaderContent()
            {
                Level = reader.ReadInt32(),
                Proto = reader.ReadInt32(1),
                Predecessor = reader.ReadBase58(Lengths.B.Decoded, Prefix.B),
                Timestamp = DateTimeExtension.FromUnixTime(reader.ReadInt64()),
                ValidationPass = reader.ReadInt32(1),
                OperationsHash = reader.ReadBase58(Lengths.LLo.Decoded, Prefix.LLo),
                Fitness = reader.ReadEnumerable(r => r.ReadHexString()).ToList().Count == 2 ? new FitnessContent() {} : new FitnessContent(),
                Context = reader.ReadBase58(Lengths.Co.Decoded, Prefix.Co)
            };

    }
}