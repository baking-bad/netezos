using System;
using System.Linq;

namespace Netezos.Keys
{
    /// <summary>
    /// Extended (hierarchical deterministic) public key
    /// </summary>
    public class HDPubKey
    {
        /// <summary>
        /// Public key
        /// </summary>
        public PubKey PubKey { get; }

        /// <summary>
        /// Public key hash
        /// </summary>
        public string Address => PubKey.Address;

        /// <summary>
        /// 32 bytes of entropy added to the public key to enable deriving secure child keys
        /// </summary>
        public byte[] ChainCode => _ChainCode.Copy();
        
        readonly byte[] _ChainCode;
        Curve Curve => PubKey.Curve;
        HDStandard HD => HDStandard.Slip10;
        ISecretStore Store => PubKey.Store;

        internal HDPubKey(PubKey pubKey, byte[] chainCode)
        {
            PubKey = pubKey ?? throw new ArgumentNullException(nameof(pubKey));
            _ChainCode = chainCode?.Copy() ?? throw new ArgumentNullException(nameof(chainCode));
            if (chainCode.Length != 32) throw new ArgumentException("Invalid chain code length", nameof(chainCode));
        }

        /// <summary>
        /// Derives an extended child key at the given index
        /// </summary>
        /// <param name="index">Index of the child key, starting from zero</param>
        /// <param name="hardened">If true, hardened derivation will be performed</param>
        /// <returns>Derived extended child key</returns>
        public HDPubKey Derive(int index, bool hardened = false)
        {
            var uind = HDPath.GetIndex(index, hardened);
            using (Store.Unlock())
            {
                var (pubKey, chainCode) = HD.GetChildPublicKey(Curve, Store.Data, _ChainCode, uind);
                return new(new(pubKey, Curve.Kind, true), chainCode);
            }
        }

        /// <summary>
        /// Derives an extended child key at the given path relative to the current key
        /// </summary>
        /// <param name="path">HD key path string, formatted like m/44'/1729'/0/0'</param>
        /// <returns>Derived extended child key</returns>
        public HDPubKey Derive(string path) => Derive(HDPath.Parse(path));

        /// <summary>
        /// Derives an extended child key at the given path relative to the current key
        /// </summary>
        /// <param name="path">HD key path</param>
        /// <returns>Derived extended child key</returns>
        public HDPubKey Derive(HDPath path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            if (!path.Any())
                return this;

            using (Store.Unlock())
            {
                var pubKey = Store.Data;
                var chainCode = _ChainCode;

                foreach (var uind in path)
                    (pubKey, chainCode) = HD.GetChildPublicKey(Curve, pubKey, chainCode, uind);

                return new(new(pubKey, Curve.Kind, true), chainCode);
            }
        }

        /// <summary>
        /// Verifies a signature of the given array of bytes
        /// </summary>
        /// <param name="data">Original data bytes</param>
        /// <param name="signature">Signature to verify</param>
        /// <returns>True if the signature is valid, otherwise false</returns>
        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        /// <summary>
        /// Verifies a signature of the given message string
        /// </summary>
        /// <param name="message">Original message string</param>
        /// <param name="signature">Signature to verify</param>
        /// <returns>True if the signature is valid, otherwise false</returns>
        public bool Verify(string message, string signature) => PubKey.Verify(message, signature);

        #region static
        /// <summary>
        /// Creates an extended (hierarchical deterministic) public key from the given public key and chain code
        /// </summary>
        /// <param name="pubKey">Public key</param>
        /// <param name="chainCode">32 bytes of entropy to be added to the public key</param>
        /// <returns>Extended public key</returns>
        public static HDPubKey FromPubKey(PubKey pubKey, byte[] chainCode) => new(pubKey, chainCode);
        #endregion
    }
}
