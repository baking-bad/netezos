using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Netezos.Encoding;

namespace Netezos.Keys
{
    public class HDPubKey : PubKey
    {
        readonly HDStandard Hd;
        const int ChainCodeLength = 32;
        readonly byte[] ChainCode = new byte[ChainCodeLength];

        internal HDPubKey(byte[] bytes, byte[] chainCode, HDStandardKind hdStandard, ECKind ecKind, bool flush = false) : base(bytes, ecKind, flush)
        {
            if (chainCode?.Length != 32)
                throw new ArgumentException("Invalid extended key length", nameof(bytes));
            Hd = HDStandard.FromKind(hdStandard);
            ChainCode = chainCode;
        }

        public HDPubKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {
                var (bytes, chainCode) = Hd.GetChildPublicKey(Curve, Store.Data, ChainCode, index);
                return new HDPubKey(bytes, chainCode, Hd.Kind, Curve.Kind);
            }
        }

        public HDPubKey Derive(HDPath path)
        {
                var result = this;
                return path.Indexes.Aggregate(result, (current, index) => current.Derive(index));
        }

        public string GetChainCodeHex()
        {
            return Hex.Convert(ChainCode);
        }
        
        #region static
        public static HDPubKey FromBytes(byte[] bytes, byte[] chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(bytes, chainCode, hdStandard, ecKind);

        public static HDPubKey FromHex(string hex, string chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(Hex.Parse(hex), Hex.Parse(chainCode), hdStandard, ecKind, true);

        public static HDPubKey FromBase64(string base64, string chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDPubKey(Base64.Parse(base64), Base64.Parse(chainCode), hdStandard, ecKind, true);
        
        public static HDPubKey FromPubKey(PubKey pubKey, byte[] chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10)
            => new HDPubKey(pubKey.GetBytes(), chainCode, hdStandard, pubKey.Curve.Kind);
        #endregion
    }
}
