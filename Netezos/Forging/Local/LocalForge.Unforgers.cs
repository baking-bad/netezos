using Netezos.Encoding;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        public static IMicheline UnforgeMicheline(ForgedReader reader)
        {
            var micheline = reader.ReadMicheline();

            if (!reader.EndOfStream)
            {
                throw new ArgumentException($"Did not reach EOS (position: {reader.StreamPosition})");
            }

            return micheline;
        }
    }
}
