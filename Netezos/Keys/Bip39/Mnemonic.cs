using System.Collections.Generic;
using System.Text.RegularExpressions;

using Netezos.Utils;

namespace Netezos.Keys
{
    public class Mnemonic
    {
        readonly string Sentence;

        public Mnemonic() : this(MnemonicSize.M15) { }
        
        public Mnemonic(MnemonicSize size)
        {
            var entropy = RNG.GetNonZeroBytes((int)size * 4 / 3);
            var words = Bip39.GetMnemonic(entropy);

            Sentence = string.Join(" ", words);
        }

        public Mnemonic(string mnemonic) => Sentence = Regex.Replace(mnemonic, @"[\s,;]+", " ");

        public Mnemonic(IEnumerable<string> words) => Sentence = string.Join(" ", words);

        public byte[] GetSeed() => Bip39.GetSeed(Sentence);

        public byte[] GetSeed(string passphrase) => Bip39.GetSeed(Sentence, passphrase);

        public override string ToString() => Sentence;

        #region static
        public static Mnemonic Parse(string mnemonic) => new Mnemonic(mnemonic);

        public static Mnemonic Parse(IEnumerable<string> words) => new Mnemonic(words);
        #endregion
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
