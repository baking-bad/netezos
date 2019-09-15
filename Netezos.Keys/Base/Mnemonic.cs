using System;
using System.Collections.Generic;
using System.Security;
using Netezos.Keys.Utils.Crypto;

namespace Netezos.Keys
{
    public class Mnemonic
    {
        private readonly SecureString MnemonicSentence;
        
        public Mnemonic()
        : this(MnemonicSize.M15)
        {}
        
        public Mnemonic(MnemonicSize size)
        {
            Bip39 bip39 = new Bip39();

            var ms = (int) size;
            var msArray = new[] { 12, 15, 18, 21, 24 };
            var entArray = new[] { 128, 160, 192, 224, 256 };
            var i = Array.IndexOf(msArray, ms);

            byte[] bytes = new byte[entArray[i] / 8];

            new Random().NextBytes(bytes);

            List<string> code = bip39.ToMnemonic(bytes);

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
