---
title: Get started
description: Short guide on how to get testnet Tezos coins with Netezos, Tezos SDK for .NET developers.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, faucet,
---

# Testnet Faucets interaction

To get started, we need an account with funds. The easiest way to get test XTZ or FA tokens is by using the [Faucet Telegram Bot](https://t.me/tezos_faucet_bot).

Let's generate a new key and get test coins from the [telegram bot](https://t.me/tezos_faucet_bot):

```csharp
// generate new key
var key = new Key();

// or use existing one
var key = Key.FromBase58("edsk4ZkGeBwDyFVjZLL2neV5FUeWNN4NJntFNWmWyEBNbRwa2u3jh1");

// use this address to receive some tez
var address = key.Address; // tz1SauKgPRsTSuQRWzJA262QR8cKdw1d9pyK
```

Let's go to the [Faucet Bot](https://t.me/tezos_faucet_bot) and get some test coins.
Click on `🤑 Get coins` to receive 100&nbsp;ꜩ to the specified tz-address, or `🍬 Get tokens` to receive some test tokens, or `➕ Add subscription` and set `Amount` to ensure your balance is always non-zero.
