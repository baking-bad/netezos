---
title: Tezos Forging & Unforging
description: Short guide on how to forge and unforge Tezos operations using Netezos, Tezos SDK for .NET developers.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, forging, unforging
---

# Tezos Forging & Unforging

Netezos supports functions to convert Tezos operations and their associated data into binary form for injection into the Tezos blockchain&mdash;a process known as **forging**. The reverse is also possible; converting binary forms of injected operations into native types&mdash;a process known as **unforging**.

## Forging

### Forge an operation

Forging an operation requires a block hash, an operation type, and any necessary operation parameters. Let's forge a transaction operation to transfer funds between two accounts.

```cs
var localForge = new LocalForge();

var blockHash = "BLghtuUq4Q578fA4STBWcSnVSBAtP1yQjtgLdejqAzzvtyrZanJ";

var operation = new TransactionContent
{
    Source = "tz1NXjqkurAmpKJEF76T58oyNsy3hWK7mk8e",
    Destination = "KT1SkmB19o8nfhRvG9LL7TjDfX2Bm1nCuYoY",
    Fee = 22100,
    Counter = 377727,
    GasLimit = 218465,
    StorageLimit = 668,
    Amount = 1
};

var forgedBytes = await localForge.ForgeOperationAsync(blockHash, operationArgs);
```

The variable `forgedBytes` will now contain the block hash, operation, and all of its parameters in binary form.

### Forge an operation group

It is also possible to forge a group of operations together as a batch. Here is an example of a `reveal` operation followed by `transaction` operation.

```cs
var localForge = new LocalForge();

var blockHash = "BLCFdxw2kWJfCk9TWQsYxrQd9CcPPs2YdbArbDDgL4GZTYvTfZN";

var operations = new ManagerOperationContent[]
{
    new RevealContent
    {
        Source = "tz1f2MeahW6XMLcfHJSU5VH8USC4EuFiwdhx",
        Fee = 1257,
        Counter = 5,
        GasLimit = 10000,
        StorageLimit = 0,
        PublicKey = "edpkuEmaQSYKgDj5k9wfE3bTxjfjoG9k5YvRmYZsGf2bjEymZKkzNn"
    },
    new TransactionContent
    {
        Source = "tz1f2MeahW6XMLcfHJSU5VH8USC4EuFiwdhx",
        Destination = "tz1f2MeahW6XMLcfHJSU5VH8USC4EuFiwdhx",
        Fee = 1188,
        Counter = 6,
        GasLimit = 10307,
        StorageLimit = 0,
        Amount = 1
    }
};

var forgedBytes = await localForge.ForgeOperationGroupAsync(blockHash, operations);
```

Likewise, the variable `forgedBytes` will now contain the block hash, batched operations, and all operation parameters in binary form.

## Unforging

Following from the previous example of forging an operation or operations, we can also unforge the forged bytes from their binary form to native types.

### Unforge single or multiple operation(s)

```cs
var localForge = new LocalForge();

var unforgedOperation = await localForge.UnforgeOperationAsync(forgedBytes);

var blockHash = unforgedOperations.Item1; // string

var operations = unforgedOperations.Item2; // IEnumerable<OperationContent>
```

Since the number of operations from forged bytes cannot be known until the unforge process actually occurs, the return type of `UnforgeOperationAsync` is a tuple type of `(string, IEnumerable<OperationContent>)`. The `string` (`unforgedOperations.Item1`) is the block hash, and the `IEnumerable<OperationContent>` (`unforgedOperations.Item2`) is an enumerable of operations that may contain 1 or more operations based on the number of forged operations. 

## Operation types

Following is a chart of Tezos operations and their operation native type counterparts.

|         Operation           |            Type            |
| :-------------------------: | :------------------------: |
| activate_account            | ActivationContent          |
| ballot                      | BallotContent              |
| delegation                  | DelegationContent          |
| double_baking_evidence      | DoubleBakingContent        |
| double_endorsement_evidence | DoubleEndorsementContent   |
| endorsement                 | EndorsementContent         |
| origination                 | OriginationContent         |
| proposals                   | ProposalsContent           |
| reveal                      | RevealContent              |
| seed_nonce_revelation       | SeedNonceRevelationContent |
| transaction                 | TransactionContent         |
