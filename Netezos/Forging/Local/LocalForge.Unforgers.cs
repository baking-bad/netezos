﻿using Netezos.Encoding;
using System;

namespace Netezos.Forging
{
    public partial class LocalForge
    {
        static IMicheline UnforgeMicheline(ForgedReader reader)
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
