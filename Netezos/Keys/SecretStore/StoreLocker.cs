namespace Netezos.Keys
{
    class StoreLocker(ISecretStore store) : IDisposable
    {
        public void Dispose() => store.Lock();
    }
}
