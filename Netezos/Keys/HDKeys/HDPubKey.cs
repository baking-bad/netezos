using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netezos.Encoding;

namespace Netezos.Keys
{
    public class HDPubKey
    {
        public PubKey PubKey
        {
            get
            {
                if (_PubKey == null)
                {
                    using (Store.Unlock())
                    {
                        //TODO: properly handle public key length for different EC
                        var publicKey = Store.Data.GetBytes(0, 32);
                        _PubKey = new PubKey(publicKey, Curve.Kind, true);
                    }
                }

                return _PubKey;
            }
        }
        PubKey _PubKey;

        readonly Curve Curve;
        readonly HDStandard Hd;
        readonly ISecretStore Store;
        
        private const int ChainCodeLength = 32;
        internal readonly byte[] vchChainCode = new byte[ChainCodeLength];


        internal HDPubKey(byte[] bytes, byte[] chainCode, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            //TODO Return back after tests
            /*if (bytes?.Length != 64)
                throw new ArgumentException("Invalid extended key length", nameof(bytes));*/

            vchChainCode = chainCode;
            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Store = new PlainSecretStore(bytes);
            if (flush) bytes.Flush();
        }

        public HDPubKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {
                var bytes = Hd.GetChildPublicKey(Curve, Store.Data, vchChainCode, index);
                return new HDPubKey(bytes.Item1, bytes.Item2, Hd.Kind, Curve.Kind);
            }
        }

        public HDPubKey Derive(HDPath path)
        {
                var result = this;
                return path.Indexes.Aggregate(result, (current, index) => current.Derive(index));
        }

        public byte[] GetBytes()
        {
            using (Store.Unlock())
            {
                var bytes = new byte[Store.Data.Length];
                Buffer.BlockCopy(Store.Data, 0, bytes, 0, Store.Data.Length);
                return bytes;
            }
        }

        public string GetChainCodeHex()
        {
            return Hex.Convert(vchChainCode);
        }
        

        #region static
        public static HDPubKey FromBytes(byte[] bytes, byte[] chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(bytes, chainCode, hdStandard, ecKind);

        public static HDPubKey FromHex(string hex, string chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(Hex.Parse(hex), Hex.Parse(chainCode), hdStandard, ecKind, true);

        public static HDPubKey FromBase64(string base64, string chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(Base64.Parse(base64), Base64.Parse(chainCode), hdStandard, ecKind, true);
        #endregion
    }
}
