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

        public Mnemonic(string mnemonic, bool skipValidation = false)
        {
            var normalized = MnemonicRegex().Replace(mnemonic, " ");
            var words = normalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!skipValidation)
                Bip39.GetEntropy(words); // Validate mnemonic
            Sentence = normalized;
        }

        public Mnemonic(IEnumerable<string> words, bool skipValidation = false)
        {
            if (!skipValidation)
                Bip39.GetEntropy(words); // Validate mnemonic
            Sentence = string.Join(" ", words);
        }

        public byte[] GetSeed() => Bip39.GetSeed(Sentence);

        public byte[] GetSeed(string passphrase) => Bip39.GetSeed(Sentence, passphrase);

        public override string ToString() => Sentence;

        #region static
        public static Mnemonic Parse(string mnemonic, bool skipValidation = false) => new(mnemonic, skipValidation);

        public static Mnemonic Parse(IEnumerable<string> words, bool skipValidation = false) => new(words, skipValidation);

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
