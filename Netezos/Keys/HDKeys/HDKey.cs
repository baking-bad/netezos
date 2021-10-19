using System;
using System.Linq;
using Netezos.Utils;

namespace Netezos.Keys
{
    //TODO Fill in returns summary
    /// <summary>
    /// Private Hierarchical Deterministic Key
    /// </summary>
    public class HDKey
    {
        /// <summary>
        /// Private Key
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// 256 bits of entropy added to the public and private keys to help them generate secure child keys
        /// </summary>
        public byte[] ChainCode => _ChainCode.Copy();

        /// <summary>
        /// Public Key
        /// </summary>
        public PubKey PubKey => Key.PubKey;

        /// <summary>
        /// Public Hierarchical Deterministic Key
        /// </summary>
        public HDPubKey HDPubKey => _HDPubKey ??= new(Key.PubKey, _ChainCode);

        /// <summary>
        /// Public Key Hash
        /// </summary>
        public string Address => Key.Address;

        readonly byte[] _ChainCode;
        HDPubKey _HDPubKey;
        Curve Curve => Key.Curve;
        HDStandard HD => HDStandard.Slip10;
        ISecretStore Store => Key.Store;

        /// <summary>
        /// Gets an elliptic curve kind and return a generated Private Hierarchical Deterministic Key
        /// </summary>
        /// <param name="kind">Elliptic curve kind. Ed25519, Secp256k1 and Nistp256 are supported</param>
        public HDKey(ECKind kind = ECKind.Ed25519)
        {
            Key = new(RNG.GetBytes(32), kind, true);
            _ChainCode = RNG.GetBytes(32);
        }

        internal HDKey(Key key, byte[] chainCode)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            _ChainCode = chainCode?.Copy() ?? throw new ArgumentNullException(nameof(chainCode));
            if (chainCode.Length != 32) throw new ArgumentException("Invalid chain code length", nameof(chainCode));
        }

        /// <summary>
        /// Derives a new extended key in the hierarchy as the given child number.
        /// </summary>
        /// <param name="index">Child number index</param>
        /// <param name="hardened">Hardened key or not (or, equivalently, whether i ≥ 2^31)</param>
        /// <returns>Derived child Hierarchical Deterministic Private Key</returns>
        public HDKey Derive(int index, bool hardened = false)
        {
            var uind = HDPath.GetIndex(index, hardened);
            using (Store.Unlock())
            {
                var (prvKey, chainCode) = HD.GetChildPrivateKey(Curve, Store.Data, _ChainCode, uind);
                return new(new(prvKey, Curve.Kind, true), chainCode);
            }
        }

        /// <summary>
        /// Derives a new extended key in the hierarchy at the given path string below the current key, by deriving the specified child at each step.
        /// </summary>
        /// <param name="path">The Key path formatted like m/44'/1729'/0/0'</param>
        /// <returns>Derived child Hierarchical Deterministic Private Key</returns>
        public HDKey Derive(string path) => Derive(HDPath.Parse(path));

        /// <summary>
        /// Derives a new extended key in the hierarchy at the given path object below the current key, by deriving the specified child at each step.
        /// </summary>
        /// <param name="path">Represent a path in the hierarchy of HD keys (BIP32)</param>
        /// <returns>Derived child Hierarchical Deterministic Private Key</returns>
        public HDKey Derive(HDPath path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (!path.Any())
                return this;

            using (Store.Unlock())
            {
                var prvKey = Store.Data;
                var chainCode = _ChainCode;

                foreach (var uind in path)
                    (prvKey, chainCode) = HD.GetChildPrivateKey(Curve, prvKey, chainCode, uind);

                return new(new(prvKey, Curve.Kind, true), chainCode);
            }
        }

        /// <summary>
        /// Gets an array of bytes and sign it with the given key. Returns a Signature object.
        /// </summary>
        /// <param name="bytes">Array of bytes</param>
        /// <returns>Signature object</returns>
        public Signature Sign(byte[] bytes) => Key.Sign(bytes);

        /// <summary>
        /// Gets a message string and sign it with the given key. Returns a Signature object.
        /// </summary>
        /// <param name="message">Any string to be signed</param>
        /// <returns>Signature object</returns>
        public Signature Sign(string message) => Key.Sign(message);

        /// <summary>
        /// Prepends forged operation bytes with 0x03 and signs the result
        /// </summary>
        /// <param name="bytes">Forged operation bytes</param>
        /// <returns>Signature object</returns>
        public Signature SignOperation(byte[] bytes) => Key.SignOperation(bytes);

        /// <summary>
        /// Gets arrays of bytes of data and signature and verify them with the given public key. Returns true if the given signature is valid.
        /// </summary>
        /// <param name="data">An array of the signed payload data</param>
        /// <param name="signature">The signature to be verified</param>
        /// <returns>True if the signature is valid</returns>
        public bool Verify(byte[] data, byte[] signature) => Key.Verify(data, signature);

        /// <summary>
        /// Gets a message string and a signature string nd verify them with the given public key. Returns true if the given signature is valid. 
        /// </summary>
        /// <param name="message">String representation of the signed payload data</param>
        /// <param name="signature">The signature to be verified</param>
        /// <returns>True if the signature is valid</returns>
        public bool Verify(string message, string signature) => Key.Verify(message, signature);

        #region static
        /// <summary>
        /// Gets a private key and a chain code and returns a Hierarchical Deterministic Private Key.
        /// </summary>
        /// <param name="key">Private Key</param>
        /// <param name="chainCode">256 bits of entropy added to the public and private keys to help them generate secure child keys</param>
        /// <returns>Private Hierarchical Deterministic Key</returns>
        public static HDKey FromKey(Key key, byte[] chainCode) => new(key, chainCode);

        /// <summary>
        /// Gets a BIP-0039 mnemonic, a passphrase and an elliptic curve kind. If a passphrase is not present, an empty string "" is used instead.
        /// Returns a Hierarchical Deterministic Private Key.
        /// </summary>
        /// <param name="mnemonic">BIP-0039 mnemonic sentence</param>
        /// <param name="passphrase">Passphrase string. If a passphrase is not present, an empty string "" is used instead.</param>
        /// <param name="kind">Elliptic curve kind. Ed25519, Secp256k1 and Nistp256 are supported</param>
        /// <returns>Private Hierarchical Deterministic Key</returns>
        public static HDKey FromMnemonic(Mnemonic mnemonic, string passphrase = "", ECKind kind = ECKind.Ed25519)
        {
            if (mnemonic == null) throw new ArgumentNullException(nameof(mnemonic));
            var seed = mnemonic.GetSeed(passphrase);
            var key = FromSeed(seed, kind);
            seed.Flush();
            return key;
        }

        /// <summary>
        /// Gets a seed byte sequence and an elliptic curve kind. Returns a Hierarchical Deterministic Private Key.
        /// </summary>
        /// <param name="seed">Seed byte sequence</param>
        /// <param name="kind">Elliptic curve kind. Ed25519, Secp256k1 and Nistp256 are supported</param>
        /// <returns>Private Hierarchical Deterministic Key</returns>
        public static HDKey FromSeed(byte[] seed, ECKind kind = ECKind.Ed25519)
        {
            if (seed == null) throw new ArgumentNullException(nameof(seed));
            var (prvKey, chainCode) = HDStandard.Slip10.GenerateMasterKey(Curve.FromKind(kind), seed);
            return new(new(prvKey, kind, true), chainCode);
        }
        #endregion
    }
}
