using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys
{
    public class Mnemonic
    {
        public Mnemonic()
        {
        }
        public Mnemonic(MnemonicSize size)
        {
        }
        public Mnemonic(string[] words)
        {
        }
        public Mnemonic(string words)
        {
        }
    }

    public enum MnemonicSize
    {
        M12 = 12,
        M15 = 15,
        M18 = 18,
        M21 = 21,
        M24 = 24,
    }
}
