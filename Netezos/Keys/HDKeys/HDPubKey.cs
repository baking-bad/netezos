using System;
using System.Collections.Generic;
using System.Text;
using Netezos.Encoding;

namespace Netezos.Keys
{
    class HDPubKey
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

        internal HDPubKey(byte[] bytes, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            if (bytes?.Length != 64)
                throw new ArgumentException("Invalid extended key length", nameof(bytes));

            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Store = new PlainSecretStore(bytes);
            if (flush) bytes.Flush();
        }

        public HDPubKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {
                //TODO: Hd.GetChildPublicKey(Curve, Store.Data, ...
                throw new NotImplementedException();
            }
        }

        public HDPubKey Derive(HDPath path)
        {
            using (Store.Unlock())
            {
                //TODO: Hd.GetChildPublicKey(Curve, Store.Data, ...
                throw new NotImplementedException();
            }
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

        #region static
        public static HDPubKey FromBytes(byte[] bytes, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(bytes, hdStandard, ecKind);

        public static HDPubKey FromHex(string hex, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(Hex.Parse(hex), hdStandard, ecKind, true);

        public static HDPubKey FromBase64(string base64, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(Base64.Parse(base64), hdStandard, ecKind, true);
        #endregion
    }
}
