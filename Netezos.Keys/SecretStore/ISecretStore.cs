using System;
using System.Collections.Generic;
using System.Text;

namespace Netezos.Keys
{
    interface ISecretStore
    {
        byte[] Secret { get; }

        void Lock();
        StoreLocker Unlock();
    }
}
