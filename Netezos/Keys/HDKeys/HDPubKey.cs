using System;
using System.Linq;

namespace Netezos.Keys
{
    /// <summary>
    /// 
    /// </summary>
    public class HDPubKey
    {
        /// <summary>
        /// 
        /// </summary>
        public PubKey PubKey { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Address => PubKey.Address;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hardened"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public HDPubKey Derive(string path) => Derive(HDPath.Parse(path));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool Verify(byte[] data, byte[] signature) => PubKey.Verify(data, signature);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool Verify(string message, string signature) => PubKey.Verify(message, signature);

        #region static
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pubKey"></param>
        /// <param name="chainCode"></param>
        /// <returns></returns>
        public static HDPubKey FromPubKey(PubKey pubKey, byte[] chainCode) => new(pubKey, chainCode);
        #endregion
    }
}
