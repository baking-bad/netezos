using System.Text.RegularExpressions;
using Netezos.Utils;

namespace Netezos.Keys
{
    public class Mnemonic
    {
        readonly string Sentence;

        public Mnemonic() : this(MnemonicSize.M24) { }
        
        public Mnemonic(MnemonicSize size)
        {
            var entropy = RNG.GetNonZeroBytes((int)size);
            var words = Bip39.GetMnemonic(entropy);
            Sentence = string.Join(" ", words);
        }

        public Mnemonic(string mnemonic) => Sentence = Regex.Replace(mnemonic, @"[\s,;]+", " ");

        public Mnemonic(IEnumerable<string> words) => Sentence = string.Join(" ", words);

        public byte[] GetSeed() => Bip39.GetSeed(Sentence);

        public byte[] GetSeed(string passphrase) => Bip39.GetSeed(Sentence, passphrase);

        public override string ToString() => Sentence;

        #region static
        public static Mnemonic Parse(string mnemonic) => new(mnemonic);

        public static Mnemonic Parse(IEnumerable<string> words) => new(words);
        #endregion
    }

    public enum MnemonicSize
    {
        M12 = 16,
        M15 = 20,
        M18 = 24,
        M21 = 28,
        M24 = 32,
    }
}
