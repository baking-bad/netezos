---
title: Smart contracts interaction
description: Short guide on how to interact with Tezos smart contracts using Netezos, Tezos SDK for .NET developers.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, smart contracts, NFT, FA2, FA1.2
---
# TL;DR
A full working code snippet of calling FA1.2 and FA2 transfers with Netezos SDK can be found at [.NET Fiddle](https://dotnetfiddle.net/8po214).
# Prerequisites

First, we need to create `Key` and `Rpc` objects. You can find a more detailed description of working with `Key` and `Rpc` 
in [Get start](get-started.html) and [Tezos Rpc](tezos-rpc.html) sections.

```cs
var rpc = new TezosRpc("https://rpc.tzkt.io/edo2net/");
var key = Key.FromBase58("edsk35mfZXZJiYUxqcmsK5K6ggg3owD2dpbRgFHp4zZzmrPy9RBdj8");
var address = key.PubKey.Address; // tz1ioz62kDw6Gm5HApeQtc1PGmN2wPBtJKUP
var FA12 = "KT1JiQhr9EXHL88U3hjJH6FkPv8wWdVYvwtg";
var FA2 = "KT1UF15SCkdvqkS6QDA5kJZqov6VGUU6vwFJ";
```

You can use the address to get some test tokens with the [Tezos Faucet Bot](https://t.me/tezos_faucet_bot) or even generate a new key.

# FA12 Token transfer example

When we've got a wallet and tokens, we can make a transfer. To make a transfer we need to send an operation so, we need `branch` and `counter`

```cs
// get a head block
var head = await rpc.Blocks.Head.Hash.GetAsync<string>();

// get account's counter
var counter = await rpc.Blocks.Head.Context.Contracts[address].Counter.GetAsync<int>();
```

Also, we need to get the script of the contract we want to interact to

```cs
// get the script of the contract from the RPC
var script = await rpc.Blocks.Head.Context.Contracts[FA12].Script.GetAsync();

// Deserialize the script code JSON string to the `IMicheline` object
var code = Micheline.FromJson(script.code);


var cs = new ContractScript(code);

```

To build the parameter for your transfer we need to know the entrypoint schema. You can check the parameter schema on the 
[TzKT contract entrypoint page](https://edo2net.tzkt.io/KT1JiQhr9EXHL88U3hjJH6FkPv8wWdVYvwtg/entrypoints) or we can get the schema from the `ContractScript`

```cs
var schemaString = cs.Entrypoints["transfer"].Humanize();
Console.WriteLine(schemaString);
```

When we know the schema, we can build the parameter for our transfer, using the `ContractScript`. 

```cs
var param = cs.BuildParameter( 
    "transfer",
    new 
    {
        from = key.PubKey.Address,
        to = "tz1XW4RqNY4WMEiSyAfb49CJTGqZK5AkrXB3",
        value = "123"
    });

var tx = new TransactionContent
{
    Source = key.PubKey.Address,
    Counter = ++counter,
    GasLimit = 100_000,
    Fee = 100_000,
    Destination = FA12,
    Parameters = new Parameters
    {
        Entrypoint = "transfer",
        Value = param
    }
};
```

That's it! Everything we need is to forge, sign and inject the operation

```cs
var opBytes = await new LocalForge().ForgeOperationAsync(head, tx);
var opSig = key.SignOperation(opBytes);

var opHash = await rpc.Inject.Operation.PostAsync(opBytes.Concat((byte[])opSig));
```
