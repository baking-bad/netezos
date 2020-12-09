using System;

namespace Netezos.Keys
{
    class PlainSecretStore : ISecretStore
    {
        public byte[] Data { get; private set; }

        public PlainSecretStore(byte[] data)
        {
            Data = new byte[data.Length];
            Buffer.BlockCopy(data, 0, Data, 0, data.Length);
        }

        public void Lock() { }

        public StoreLocker Unlock() => new StoreLocker(this);
    }
}
