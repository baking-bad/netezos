namespace Netezos.Keys
{
    class StoreLocker : IDisposable
    {
        readonly ISecretStore Store;

        public StoreLocker(ISecretStore store) => Store = store;

        public void Dispose() => Store.Lock();
    }
}
