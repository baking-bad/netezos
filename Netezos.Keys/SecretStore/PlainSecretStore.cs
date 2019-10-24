using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys
{
    class PlainSecretStore : ISecretStore
    {
        public byte[] Secret { get; private set; }

        public PlainSecretStore(byte[] data)
        {
            Secret = new byte[data.Length];
            Buffer.BlockCopy(data, 0, Secret, 0, data.Length);
        }

        public void Lock() { }

        public StoreLocker Unlock() => new StoreLocker(this);
    }
}
