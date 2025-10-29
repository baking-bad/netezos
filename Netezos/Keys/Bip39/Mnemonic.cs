using System.Text.RegularExpressions;
using Netezos.Utils;

namespace Netezos.Keys
{
    public partial class Mnemonic
    {
        readonly string Sentence;

        public Mnemonic() : this(MnemonicSize.M24) { }
        
        public Mnemonic(MnemonicSize size)
        {
            var entropy = RNG.GetNonZeroBytes((int)size);
            var words = Bip39.GetMnemonic(entropy);
            Sentence = string.Join(" ", words);
        }

        public Mnemonic(string mnemonic)
        {
            var normalized = MnemonicRegex().Replace(mnemonic, " ");
            var words = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            Bip39.GetEntropy(words); // Validate mnemonic
            Sentence = normalized;
        }

        public Mnemonic(IEnumerable<string> words)
        {
            Bip39.GetEntropy(words); // Validate mnemonic
            Sentence = string.Join(" ", words);
        }

        public byte[] GetSeed() => Bip39.GetSeed(Sentence);

        public byte[] GetSeed(string passphrase) => Bip39.GetSeed(Sentence, passphrase);

        public override string ToString() => Sentence;

        #region static
        public static Mnemonic Parse(string mnemonic) => new(mnemonic);

        public static Mnemonic Parse(IEnumerable<string> words) => new(words);

        [GeneratedRegex(@"[\s,;]+")]
        private static partial Regex MnemonicRegex();
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
