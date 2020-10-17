namespace Netezos.Keys
{
    interface ISecretStore
    {
        byte[] Data { get; }

        void Lock();
        StoreLocker Unlock();
    }
}
