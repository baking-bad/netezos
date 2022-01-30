---
title: Get started
description: Short guide on how to get testnet Tezos coins with Netezos, Tezos SDK for .NET developers.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, faucet,
---

# Testnet Faucets interaction

To get started, we need an account with funds. There are two ways to do this: to activate a new account from a JSON file, or a much simpler way to get money from the [Faucet Telegram Bot](https://t.me/tezos_faucet_bot). Let’s consider both of them.

## Using faucet bot

Let’s generate a new key and get test coins from the telegram bot:

```csharp
// generate new key
var key = new Key();

// or use existing one
var key = Key.FromBase58("edsk4ZkGeBwDyFVjZLL2neV5FUeWNN4NJntFNWmWyEBNbRwa2u3jh1");

// use this address to receive some tez
var address = key.Address; // tz1SauKgPRsTSuQRWzJA262QR8cKdw1d9pyK
```

Let’s go to the [Faucet Bot](https://t.me/tezos_faucet_bot) and get some test coins. You can use the `🤑 Get coins` option to deposit 100&nbsp;ꜩ to your tz address, or `➕ Add subscription` and set `Amount` to ensure your balance is always non-zero.

## Activation via JSON

First of all, let’s download a JSON file from [https://teztnets.xyz/](https://teztnets.xyz/ "https://teztnets.xyz/") and parse the data:

> _NOTE: Pick the faucet of the network where you want to get the test coins, for example "Ithacanet faucet"_

```csharp
var words = new List<string>
{
    "arrive", "acquire", "salt", "salt", "slight", "enforce", "admit", "basket", 
    "empty", "zebra", "general", "dice", "chaos", "brisk", "champion"
};
var activation_code = "0265a665ad77d46ba9a7a6c918cae503912267a3";
var password = "cFSRvdtjcs";
var email = "rdvmizpp.duiekjyz@teztnets.xyz";
```

We can use that to extract the private key:

```csharp
var mnemonic = new Mnemonic(words);
var key = Key.FromMnemonic(mnemonic, email, password);
```

Now, that we have received the key, we can activate the account. We will use [Netezos.Rpc](../api/Netezos.Rpc.html) for that purpose. Read all about it in the [Tezos Rpc](tezos-rpc.html) section.

## Account activation

Let’s create the content object for our future operation. The `pkh` and `activation_code` fields are taken from the downloaded JSON file:

```csharp
var activation = new ActivationContent()
{
    Address = key.Address,
    Secret = activation_code
};
```

Then let’s create an RPC object and get the required data from the chain:

```csharp
var rpc = new TezosRpc("https://rpc.tzkt.io/ithacanet/");
var branch = await rpc.Blocks.Head.Hash.GetAsync<string>();
var bytes = await new LocalForge().ForgeOperationAsync(branch, activation);
```

Once we get operation bytes we can sign them with our key and broadcast to the network:

```csharp
var signature = (byte[])key.SignOperation(bytes);
var result = await rpc.Inject.Operation.PostAsync(bytes.Concat(signature));
```

