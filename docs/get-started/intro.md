<img="images/dotNetTZ_violet.svg"></img>
## Getting started

Let's consider the most common use case - sending a transaction.

### Installation

`PM> Install-Package Netezos`

### Create private key

````cs
// generate new key
var key = new Key();

// or use existing one
var key = Key.FromBase58("edsk4ZkGeBwDyFVjZLL2neV5FUeWNN4NJntFNWmWyEBNbRwa2u3jh1");

// use this address to receive some tez
var address = key.PubKey.Address; // tz1SauKgPRsTSuQRWzJA262QR8cKdw1d9pyK
````

### Get some data from RPC

````cs
using var rpc = new TezosRpc("https://mainnet-tezos.giganode.io/");

// get a head block
var head = await rpc.Blocks.Head.Hash.GetAsync<string>();

// get account's counter
var counter = await rpc.Blocks.Head.Context.Contracts[address].Counter.GetAsync<int>();
````

### Forge an operation

Since our address has just been created, we need to reveal its public key before sending any operation, so that everyone can validate our signatures.
Therefore, we need to send actually two operations: a reveal and then a transaction.

Netezos allows you to pack multiple operations into a group and forge/send it as a single batch.

````cs
var content = new ManagerOperationContent[]
{
    new RevealContent
    {
        Source = address,
        Counter = ++counter,
        PublicKey = key.PubKey.GetBase58(),
        GasLimit = 1500,
        Fee = 1000 // 0.001 tez
    },
    new TransactionContent
    {
        Source = address,
        Counter = ++counter,
        Amount = 1000000, // 1 tez
        Destination = "tz1KhnTgwoRRALBX6vRHRnydDGSBFsWtcJxc",
        GasLimit = 1500,
        Fee = 1000 // 0.001 tez
    }
};

var bytes = await new LocalForge().ForgeOperationGroupAsync(head, content);
````

### Sign and send

````cs
// sign the operation bytes
byte[] signature = key.SignOperation(bytes);

// inject the operation and get its id (operation hash)
var result = await rpc.Inject.Operation.PostAsync(bytes.Concat(signature));
````

That is it. We have successfully injected our first operation into the Tezos blockchain.

## Useful links

- [Examples of Netezos usage](https://baking-bad.org/blog/2019/11/14/tezos-c-sdk-examples-of-netezos-usage/)
- [Forge an operation locally and sign it using Ledger](https://baking-bad.org/blog/2019/12/30/tezos-c-sdk-netezos-forge-an-operation-locally-and-sign-it-using-ledger-wallet/)