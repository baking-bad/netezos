---
title: Wallet Import and HD Keys Interaction
description: Short guide on how to create or import a Hierarchical Deterministic (HD) wallet.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, hierarchical deterministic, hd, kukai, atomex, temple wallet, slip-0010, bip-32
---

# Wallet Import and HD Keys Interaction
With Netezos.Keys you can create or import a Hierarchical Deterministic (HD) wallet.

## Basic usage

In order to create an HD wallet, you just need to create a new instance of `HDKey`

```cs
var masterKey = new HDKey(ECKind.Ed25519); //Ed25519, Secp256k1, and NistP256 are available
```

To get a child key, for instance, with the path `"m/44'/1729'/0'/0'"` (it's a default path for Atomex, Temple, Kukai and other Tezos wallets), we need to `Derive` it from the master key

```cs
var childHdKey = masterKey.Derive("m/44'/1729'/0'/0'");
```

Also, if you need to get a lot of child keys, you can derive an intermediate HD key and then derive children from it, using a shorter derivation path, for slightly better performance:

```cs
var hdKey = masterKey.Derive("m/44'/1729'");

var firstChild = hdKey.Derive("/0'/0'"); // the result path is m/44'/1729'/0'/0'
// or
var secondChild = hdKey.Derive(1, true).Derive(0); // the result path is m/44'/1729'/1'/0
// etc.
```

## Public Key Derivation

Public key derivation process is identical to the private key derivation, but keep in mind that it always fails for `Ed25519` and for other curves it is only defined for non-hardened child keys.
To create an `HDPubKey` object we need a public key and a chain code

```cs
var hdKey = new HDKey(ECKind.NistP256);
var hdPubKey = HDPubKey.FromPubKey(hdKey.PubKey, hdKey.ChainCode);
var childPubKey = hdPubKey.Derive("m/44/1729/0/0");
var anotherChild = childPubKey.Derive(0);
```
