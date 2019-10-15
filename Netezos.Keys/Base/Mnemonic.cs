using System.Collections.Generic;
using System.Security;
using Netezos.Keys.Utils.Crypto;

namespace Netezos.Keys
{
    public class Mnemonic
    {
        readonly SecureString MnemonicSentence;
        
        public Mnemonic()
        : this(MnemonicSize.M15)
        {}
        
        public Mnemonic(MnemonicSize size)
        {
            Bip39 bip39 = new Bip39();

            List<string> code = bip39.ToMnemonic(RNG.GetNonZeroBytes((int)size*11*32/33 / 8));

            MnemonicSentence = string.Join(" ", code).ToSecureString();
            
        }
        public Mnemonic(string[] words)
        {
            MnemonicSentence = string.Join(" ", words).ToSecureString();
        }
        public Mnemonic(string words)
        {
            MnemonicSentence = words.ToSecureString();
        }

        public byte[] GetSeed(string passphrase)
        {
            return Bip39.ToSeed(MnemonicSentence.SecureStringToString(), passphrase).GetBytes(0, 32);
        }

        public byte[] GetSeed() => GetSeed("");
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
