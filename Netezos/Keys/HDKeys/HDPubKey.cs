using System;
using System.Linq;

namespace Netezos.Keys
{
    /// <summary>
    /// Public Hierarchical Deterministic Key
    /// </summary>
    public class HDPubKey
    {
        /// <summary>
        /// Public Key
        /// </summary>
        public PubKey PubKey { get; }

        /// <summary>
        /// Public Key Hash
        /// </summary>
        public string Address => PubKey.Address;

        /// <summary>
        /// 256 bits of entropy added to the public key to help it generate secure child keys
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
        /// Derives a new extended public key in the hierarchy as the given child index.
        /// </summary>
        /// <param name="index">Child index</param>
        /// <param name="hardened">Hardened key or not (index | 0x80000000 will be performed)</param>
        /// <returns>Derived child Hierarchical Deterministic Public Key</returns>
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
        /// Derives a new extended public key in the hierarchy at the given path string below the current key, by deriving the specified child at each step.
        /// </summary>
        /// <param name="path">The key path formatted like m/44'/1729'/0'/0'</param>
        /// <returns>Derived child Hierarchical Deterministic Public Key</returns>
        public HDPubKey Derive(string path) => Derive(HDPath.Parse(path));

        /// <summary>
        /// Derives a new extended public key in the hierarchy at the given path object below the current key, by deriving the specified child at each step.
        /// </summary>
        /// <param name="path">HDPath object</param>
        /// <returns>Derived child Hierarchical Deterministic Public Key</returns>
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
        /// Gets arrays of bytes of data and signature and verify them with the given public key.
        /// </summary>
        /// <param name="data">An array of the signed payload data</param>
        /// <param name="signature">The signature to be verified</param>
        /// <returns>True if the signature is valid. Otherwise false</returns>
        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        /// <summary>
        /// Gets a message string and a signature string and verify them with the given public key.
        /// </summary>
        /// <param name="message">String representation of the signed payload data</param>
        /// <param name="signature">The signature to be verified</param>
        /// <returns>True if the signature is valid. Otherwise false</returns>
        public bool Verify(string message, string signature) => PubKey.Verify(message, signature);

        #region static
        /// <summary>
        /// Gets a public key and a chain code and returns a Hierarchical Deterministic Public Key.
        /// </summary>
        /// <param name="pubKey">Public Key</param>
        /// <param name="chainCode">256 bits of entropy added to the public key to help it generate secure child keys</param>
        /// <returns>Public Hierarchical Deterministic Key</returns>
        public static HDPubKey FromPubKey(PubKey pubKey, byte[] chainCode) => new(pubKey, chainCode);
        #endregion
    }
}
