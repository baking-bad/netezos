using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys
{
    interface ISecretStore : IDisposable
    {
        byte[] Data { get; }

        void Lock();
        StoreLocker Unlock();
    }
}
