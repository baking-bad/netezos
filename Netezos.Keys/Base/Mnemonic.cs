using Netezos.Keys.Utils.Crypto;

namespace Netezos.Keys
{
    public class Mnemonic
    {
        readonly string Sentence;

        public Mnemonic() : this(MnemonicSize.M15) { }
        
        public Mnemonic(MnemonicSize size)
        {
            var entropy = RNG.GetNonZeroBytes((int)size * 4 / 3);
            var words = Bip39.ToMnemonic(entropy);

            Sentence = string.Join(" ", words);
        }

        public Mnemonic(string words) => Sentence = words;

        public Mnemonic(string[] words) => Sentence = string.Join(" ", words);

        public byte[] GetSeed() => Bip39.ToSeed(Sentence, "");

        public byte[] GetSeed(string passphrase) => Bip39.ToSeed(Sentence, passphrase);

        public override string ToString() => Sentence;
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
