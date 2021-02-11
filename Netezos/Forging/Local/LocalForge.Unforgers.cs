using Netezos.Encoding;
using System;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static IMicheline UnforgeMicheline(byte[] data)
        {
            using (MichelineReader mr = new MichelineReader(data))
            {
                return UnforgeMicheline(data);
            }
        }

        static IMicheline UnforgeMicheline(MichelineReader reader)
        {
            IMicheline micheline = reader.ReadMicheline();

            if (!reader.EndOfStream)
            {
                throw new ArgumentException($"Did not reach EOS (position: {reader.StreamPosition})");
            }

            return micheline;
        }
    }
}
