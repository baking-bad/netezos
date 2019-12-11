using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LedgerWallet;
using LedgerWallet.Transports;
using NBitcoin;
using Netezos.Keys;
using PubKey = Netezos.Keys.PubKey;

namespace Netezos.Signer
{
    public class TezosLedgerClient : LedgerClientBase
    {
        byte TezosWalletCLA = 0x80;
        byte[] KeyPath;
        
        enum Instruction { // taken from https://github.com/obsidiansystems/ledger-app-tezos/blob/master/APDUs.md
            InsVersion = 0x00,
            InsGetPublicKey = 0x02,
            InsPromptPublicKey = 0x03,
            InsSign = 0x04,
            InsSignUnsafe = 0x05
        }

        public TezosLedgerClient(ILedgerTransport transport, KeyPath keyPath) : base(transport)
        {
            KeyPath = keyPath == null
                ? Utils.Serialize(new KeyPath("44'/1729'/0'/0'"))
                : Utils.Serialize(keyPath);
        }

        public static async Task<IEnumerable<TezosLedgerClient>> GetHIDLedgersAsync(KeyPath keyPath = null)
        {
            var ledgers = (await HIDLedgerTransport.GetHIDTransportsAsync())
                .Select(t => new TezosLedgerClient(t, keyPath))
                .ToList();
            return ledgers;
        }
        
        public async Task<PubKey> GetWalletPubKeyBytesAsync(ECKind curve = ECKind.Ed25519, bool display = false, CancellationToken cancellation = default(CancellationToken))
        {
            var response = await ExchangeSingleAPDUAsync(
                    TezosWalletCLA,
                    (byte) Instruction.InsGetPublicKey,
                    (byte)(display ? 1 : 0),
                    (byte) (curve - 1) , KeyPath, OK, cancellation)
                .ConfigureAwait(false);
            return PubKey.FromBytes(Utils.GetBytes(response,2, response.Length - 2));
        }
        
        public async Task<Signature> Sign(byte[] data, ECKind curve = ECKind.Ed25519, CancellationToken cancellation = default)
        {
            var offset = 0;
            var maxSize = 230;
            var response = new byte[]{};

            var list = new List<byte[]> {KeyPath};
            
            list.AddRange(Utils.SplitArray(data));
            
            foreach (var (value, index) in list.Select((value, i) => ( value, i )))
            {
                var code = (index == 0)
                    ? 0x00
                    : (index == list.Count - 1)
                        ? 0x81
                        : 0x01;
                response = await ExchangeSingleAPDUAsync(
                    TezosWalletCLA,
                    (byte) Instruction.InsSign,
                    (byte) code,
                    (byte) (curve - 1), value, OK, cancellation).ConfigureAwait(false);
            }
	
            return new Signature(response, Utils.GetSignaturePrefix(curve));
        }
    }
}