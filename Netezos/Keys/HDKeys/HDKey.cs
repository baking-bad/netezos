﻿using System;
using System.ComponentModel;
using System.Linq;
using Netezos.Encoding;
using Netezos.Utils;

namespace Netezos.Keys
{
    public class HDKey
    {
        public Key Key
        {
            get
            {
                if (_Key == null)
                {
                    using (Store.Unlock())
                    {
                        //TODO Changed lenght from 32 to 64 for testing purposes. Change back after tests
                        var privateKey = Store.Data.GetBytes(0, 32);
                        _Key = new Key(privateKey, Curve.Kind, true, _PubKey);
                    }
                }

                return _Key;
            }
        }
        Key _Key;
        
        public PubKey PubKey
        {
            get
            {
                if (_PubKey == null)
                {
                    using (Store.Unlock())
                    {
                        _PubKey = new PubKey(Curve.GetPublicKey(Store.Data.GetBytes(0,32)), Curve.Kind, true);
                        HdPubKey = new HDPubKey(Curve.GetPublicKey(Store.Data.GetBytes(0,32)), Store.Data.GetBytes(32,32), Hd.Kind, Curve.Kind, true);
                    }
                }

                return _PubKey;
            }
        }

        public HDPubKey HdPubKey;
        PubKey _PubKey;

        readonly Curve Curve;
        readonly HDStandard Hd;
        readonly ISecretStore Store;
        readonly uint hardenedOffset = 0x80000000;
        public byte[] ChainCode;

        public HDKey() : this(HDStandardKind.Slip10, ECKind.Ed25519) { }

        public HDKey(HDStandardKind hdStandard) : this(hdStandard, ECKind.Ed25519) { }

        public HDKey(HDStandardKind hdStandard, ECKind ecKind)
        {
            //TODO Check the generation
            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            var bytes = RNG.GetBytes(64);
            Store = new PlainSecretStore(bytes.GetBytes(0,32));
            ChainCode = bytes.GetBytes(32, 32);
            HdPubKey = new HDPubKey(Curve.GetPublicKey(bytes.GetBytes(0, 32)), bytes.GetBytes(32,32), Hd.Kind, Curve.Kind, true);
            bytes.Flush();
        }

        public HDKey(byte[] privateKey, byte[] chainCode, HDStandardKind hdStandard, ECKind ecKind, bool flush = false)
        {
            if (privateKey?.Length != 32)
                throw new ArgumentException("Invalid private key length", nameof(privateKey));
            if (chainCode?.Length != 32)
                throw new ArgumentException("Invalid chainCode  length", nameof(chainCode));

            Curve = Curve.FromKind(ecKind);
            Hd = HDStandard.FromKind(hdStandard);
            Store = new PlainSecretStore(privateKey);
            ChainCode = chainCode;
            //TODO Consider moving out the chain code
            //TODO That's working with zero bytes for ed25519 and doesn't for secp256k1
            _PubKey = new PubKey(Curve.GetPublicKey(privateKey), Curve.Kind, flush);
            
            HdPubKey = new HDPubKey(Curve.GetPublicKey(privateKey.GetBytes(0, 32)), chainCode, Hd.Kind, Curve.Kind, true);
            
            if (flush) privateKey.Flush();
        }

        public HDKey Derive(uint index, bool hardened = false)
        {
            using (Store.Unlock())
            {
                var (privateKey, chainCode) = Hd.GetChildPrivateKey(Curve, Store.Data, ChainCode, index);
                return new HDKey(privateKey, chainCode, Hd.Kind, Curve.Kind);
            }
        }

        public HDKey Derive(HDPath path) 
        {
            using (Store.Unlock())
            {
                var (privateKey, chainCode) = path.Indexes
                    .Aggregate((Store.Data, ChainCode), (mks, next) => Hd.GetChildPrivateKey(Curve, mks.Data, mks.ChainCode, next));

                return new HDKey(privateKey, chainCode, Hd.Kind, Curve.Kind);
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

        public static HDKey FromBytes(byte[] bytes, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var (privateKey, chainCode) = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), bytes);
            return new HDKey(privateKey, chainCode, hdStandard, ecKind, true);
        }

        public static HDKey FromHex(string hex, HDStandardKind hdStandard = HDStandardKind.Slip10,
            ECKind ecKind = ECKind.Ed25519)
        {
            var (privateKey, chainCode) = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), Hex.Parse(hex));
            return new HDKey(privateKey, chainCode, hdStandard, ecKind, true);
        }

        public static HDKey FromBase64(string privateKey, string chainCode, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
            => new HDKey(Base64.Parse(privateKey), Base64.Parse(chainCode), hdStandard, ecKind, true);

        public static HDKey FromSeed(byte[] seed, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var (privateKey, chainCode) = HDStandard.FromKind(hdStandard).GenerateMasterKey(Curve.FromKind(ecKind), seed);
            return new HDKey(privateKey, chainCode, hdStandard, ecKind, true);
        }

        public static HDKey FromMnemonic(Mnemonic mnemonic, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var seed = mnemonic.GetSeed();
            var key = FromSeed(seed, hdStandard, ecKind);
            seed.Flush();
            return key;
        }

        public static HDKey FromMnemonic(Mnemonic mnemonic, string passphrase, HDStandardKind hdStandard = HDStandardKind.Slip10, ECKind ecKind = ECKind.Ed25519)
        {
            var seed = mnemonic.GetSeed(passphrase);
            var key = FromSeed(seed, hdStandard, ecKind);
            seed.Flush();
            return key;
        }
        #endregion
    }
}
