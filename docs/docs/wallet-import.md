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
var hdKey = new HDKey(ECKind.Ed25519); //Ed25519, Secp256k1, and NistP256 are available
```

To get a child key, for instance, with path `"m/44'/1729'/0'/0'"` (default path for atomex, temple, kukai and other), we need to `Derive` our `hdKey`

```cs
var childHdKey = hdKey.Derive("m/44'/1729'/0'/0'");
```

We don't need to derive from the master to get more keys, we can just use indexes to iterate:

```cs
var hdKey = new HDKey().Derive("m/44'/1729'");
var firstChild = hdKey.Derive(0, true).Derive(0, true);
var secondChild = hdKey.Derive(1, true).Derive(0, true);
```
That way, the `secondChild` path will be `"m/44'/1729'/1'/0'"` (default derivation for atomex, temple, kukai and other).
## Public Key Derivation

Public key derivation process is identical to the private key derivation, but keep in mind that it always fails for `Ed25519` and it is only defined for non-hardened child keys.
To create an `HDPubKey` object we need a public key and a child code

```cs
var hdKey = new HDKey(ECKind.NistP256);
var hdPubKey = HDPubKey.FromPubKey(hdKey.PubKey, hdKey.ChainCode);
var childPubKey = hdPubKey.Derive("m/44/1729/0/0");
var anotherChild = childPubKey.Derive(0, false);
```
