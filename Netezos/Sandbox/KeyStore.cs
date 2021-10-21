using System.Collections.Generic;
using System.Linq;
using Netezos.Keys;

namespace Netezos.Sandbox
{
    public class KeyStore
    {
        private IEnumerable<Key> Keys;

        private IDictionary<string, Key> KeysByAliases;

        public CommitmentKey ActivatorKey { get; }

        public KeyStore(IEnumerable<Key> keys) => Keys = keys;

        public KeyStore(IDictionary<string, Key> aliasesKey, CommitmentKey commitmentKey)
            : this(null, aliasesKey, commitmentKey)
        {
        }

        public KeyStore(IDictionary<string, string> aliasesKey, CommitmentKey commitmentKey) 
            : this(null, 
                aliasesKey.ToDictionary(x => x.Key, y => Key.FromBase58(y.Value)),
                commitmentKey)
        {
        }

        public KeyStore(IEnumerable<Key> keys, IDictionary<string, Key> aliasesKey, CommitmentKey commitmentKey = null)
        {
            KeysByAliases = aliasesKey;
            Keys = keys?.Union(aliasesKey.Values) ?? aliasesKey?.Values;
            ActivatorKey = commitmentKey;
        }

        public KeyStore(CommitmentKey key) => ActivatorKey = key;

        public Key this[string aliasOrPublicKey]
            => Keys.FirstOrDefault(x => x.GetBase58().Equals(aliasOrPublicKey)) 
               ?? (KeysByAliases.TryGetValue(aliasOrPublicKey, out var res) 
                   ? res 
                   : throw new KeyNotFoundException($"Key {aliasOrPublicKey} not found on Store"));

    }

    public class CommitmentKey : Key
    {
        public string ActivationCode { get; }

        internal CommitmentKey(byte[] bytes, ECKind kind, bool flush = false, string activationCode = null)
            : base(bytes, kind, flush)
        {
            ActivationCode = activationCode;
        }

        public static CommitmentKey FromMnemonic(Mnemonic mnemonic, string email, string password, string activationCode)
        {
            var seed = mnemonic.GetSeed($"{email}{password}");
            var key = new CommitmentKey(seed.GetBytes(0, 32), ECKind.Ed25519, true, activationCode);
            seed.Flush();
            return key;
        }
    }
}