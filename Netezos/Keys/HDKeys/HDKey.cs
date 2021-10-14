using System;
using System.Linq;
using Netezos.Utils;

namespace Netezos.Keys
{
    /// <summary>
    /// 
    /// </summary>
    public class HDKey
    {
        /// <summary>
        /// 
        /// </summary>
        public Key Key { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] ChainCode => _ChainCode.Copy();

        /// <summary>
        /// 
        /// </summary>
        public PubKey PubKey => Key.PubKey;

        /// <summary>
        /// 
        /// </summary>
        public HDPubKey HDPubKey => _HDPubKey ??= new(Key.PubKey, _ChainCode);

        /// <summary>
        /// 
        /// </summary>
        public string Address => Key.Address;

        readonly byte[] _ChainCode;
        HDPubKey _HDPubKey;
        Curve Curve => Key.Curve;
        HDStandard HD => HDStandard.Slip10;
        ISecretStore Store => Key.Store;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
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
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hardened"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public HDKey Derive(string path) => Derive(HDPath.Parse(path));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public Signature Sign(byte[] bytes) => Key.Sign(bytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public Signature Sign(string message) => Key.Sign(message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public Signature SignOperation(byte[] bytes) => Key.SignOperation(bytes);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool Verify(byte[] data, byte[] signature) => Key.Verify(data, signature);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool Verify(string message, string signature) => Key.Verify(message, signature);

        #region static
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="chainCode"></param>
        /// <returns></returns>
        public static HDKey FromKey(Key key, byte[] chainCode) => new(key, chainCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <param name="passphrase"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static HDKey FromMnemonic(Mnemonic mnemonic, string passphrase = "", ECKind kind = ECKind.Ed25519)
        {
            if (mnemonic == null) throw new ArgumentNullException(nameof(mnemonic));
            var seed = mnemonic.GetSeed(passphrase);
            var key = FromSeed(seed, kind);
            seed.Flush();
            return key;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static HDKey FromSeed(byte[] seed, ECKind kind = ECKind.Ed25519)
        {
            if (seed == null) throw new ArgumentNullException(nameof(seed));
            var (prvKey, chainCode) = HDStandard.Slip10.GenerateMasterKey(Curve.FromKind(kind), seed);
            return new(new(prvKey, kind, true), chainCode);
        }
        #endregion
    }
}
