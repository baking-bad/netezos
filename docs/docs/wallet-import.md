---
title: Wallet Import and HD Keys Interaction
description: Short guide on how to create or import a Hierarchical Deterministic (HD) wallet.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, hierarchical deterministic, hd, kukai, atomex, temple wallet
---

# Wallet Import and HD Keys Interaction
With Netezos.Keys you can create or import a Hierarchical Deterministic (HD) wallet.

## Basic usage

In order to create an HD wallet, you just need to create a new instance of `HDKey`

```cs
var hdKey = new HDKey(ECKind.Ed25519); //Ed25519, Secp256k1, and NistP256 are available
```

To get a child key, for instance, with path `"m/44'/1729'/0'/0'"` (default path for atomex, temple, kukai and other), we need to `Derive` our `hdKey`

```cs
var childHdKey = hdKey.Derive("m/44'/1729'/0'/0'");
```
We don't need to derive from the master to get more keys, we can just use indexes to iterate
!!! You need to check that first.