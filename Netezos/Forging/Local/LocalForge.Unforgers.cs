using Netezos.Encoding;
using System;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static IMicheline UnforgeMicheline(byte[] data)
        {
            IMicheline micheline;

            using (MichelineReader mr = new MichelineReader(data))
            {
                micheline = mr.ReadMicheline();

                if (!mr.EndOfStream)
                {
                    throw new ArgumentException($"Did not reach EOS (pos {mr.StreamPosition}/{data.Length})");
                }
            }

            return micheline;
        }
    }
}
