using System;

namespace Netezos.Keys
{
    class StoreLocker : IDisposable
    {
        ISecretStore Store;

        public StoreLocker(ISecretStore store) => Store = store;

        public void Dispose() => Store.Lock();
    }
}
