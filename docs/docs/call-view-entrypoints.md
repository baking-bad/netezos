---
title: Get data from view entrypoints
description: Short guide on how to get data from Tezos smart contracts via "view" entrypoints using Netezos, Tezos SDK for .NET developers.
keywords: netezos, tezos, tezos sdk, tezos csharp, tezos csharp sdk, blockchain, blockchain sdk, smart contracts, NFT, FA2, FA1.2
---
 
# Get data from "view" entrypoints
 
Often we need to access smart contract storage, to get specific data like token balance for a specific account, or total supply of some token, etc. 
 
In Tezos there are two ways to get data from a smart contract:
- get the whole storage and try to find there raw data;
- use "view" entrypoints to get processed data.
 
Let's see how to work with "view" entrypoints (from the outside).
 
## View entrypoints
 
A `view` is an entrypoint that represents a computation that does not modify smart contract's state, but returns some result.
In Tezos `view` entrypoints are actually the same entrypoints (technically), but following some conventions. 
See more details [here](https://gitlab.com/tzip/tzip/-/blob/master/proposals/tzip-4/tzip-4.md#view-entrypoints).
 
In contrast with `view` functions in Ethereum, which are very easy to use, `view` entrypoints in Tezos are much more complicated, because they do not return
a value directly, but pass it to the callback contract.
 
## Callback contracts
 
A `callback` is a contract (or an entrypoint of the contract, to be precise) which is used to process a result from the invoked `view` entrypoint.
Obviously, a `callback` must have the same type as the result from a `view` entrypoint.
If you pass a result of type `a` to a callback of type `b` you will get a runtime error.
 
## How it works
 
Let's see how it works in a real example.
 
This is a transaction that calls `getBalance` view entrypoint with parameter `tz1io...BtJKUP` and callback contract `KT1Hk...LDcKrq`.
Note, by appending `%viewNat` to the contract address we can specify a particular entrypoint of the contract that should be used as a callback.
 
```json
{
  "kind": "transaction",
  "source": "...",
  "destination": "KT1WUnvcNAnahbuNizZaaAvkpNAEF43KHV7F",
  "parameters": {
    "entrypoint": "getBalance",                                    // call view entrypoint
    "value": {
      "prim": "Pair",
      "args": [
        {
          "string": "tz1ioz62kDw6Gm5HApeQtc1PGmN2wPBtJKUP"         // with some args
        },
        {
          "string": "KT1DjH58WumESHhpViiwuRE9ehUY5RGMNcuL%viewNat" // and callback contract
        }
      ]
    }
  },
  "metadata": {
    "internal_operation_results": [                                // callback transaction
      {
        "kind": "transaction",
        "source": "KT1WUnvcNAnahbuNizZaaAvkpNAEF43KHV7F",
        "destination": "KT1DjH58WumESHhpViiwuRE9ehUY5RGMNcuL",     // to callback contract
        "parameters": {
          "entrypoint": "viewNat",                                 // to callback entrypoint
          "value": {
            "int": "98770"                                         // with `getBalance` result
          }
        },
      }
    ]
  }
}
```
 
So, we called `view` entrypoint by sending a transaction to a smart contract, then the smart contract produced
an internal transaction to the `callback` contract with a result value in the parameters.
 
## Call view entrypoints with Netezos
 
We've just seen how `view` entrypoints work in Tezos: you send a transaction to the smart contract, then the smart contract produces
an internal transaction with result value in its parameters and sends it to the `callback` contract. 
With Netezos we will do basically the same things.

Let's see, how to get [FA1.2 token](https://hangzhou2net.tzkt.io/KT1WUnvcNAnahbuNizZaaAvkpNAEF43KHV7F/code)
balance by calling `getBalance` entrypoint.
 
### Prepare callback contract
 
First of all, we will need an existing contract that we will use as a `callback`.
Moreover, the `callback` contract must have an entrypoint with the same parameter type as the type of the `view` entrypoint we are calling.
 
Here is how the `getBalance` entrypoint of [our contract](https://hangzhou2net.tzkt.io/KT1WUnvcNAnahbuNizZaaAvkpNAEF43KHV7F/code) looks:
 
```
(pair %getBalance (address %viewParam) (contract %viewCallbackTo nat))
```
As we can see, it takes an `address` and returns `nat`, that is passed to the callback contract.
So, we will need a `callback` contract with an entrypoint of type `nat`.
 
Where do we get such a contract? Well, we can either originate a new one or use any of already existing ones that have an entrypoint of the required type.

Let's take [existing one](https://hangzhou2net.tzkt.io/KT1DjH58WumESHhpViiwuRE9ehUY5RGMNcuL/code):
 
```
parameter (or (nat %viewNat) (or (string %viewString) (address %viewAddress)));
storage unit;
code { FAILWITH }
```
 
That contract contains the entrypoint `(nat %viewNat)` - that's exactly what we need.
 
### Simulate operation
 
Of course, we don't want to send a transaction just to get some data from the smart contract.
It would be weird to wait a minute unless a transaction is included into a block and moreover to pay a tx fee.
 
A common workaround is to use [/run_operation](https://gitlab.com/tezos/tezos/-/blob/master/docs/api/hangzhou-openapi.json) RPC endpoint
to simulate the transaction and see its result without injecting it into the blockchain, so we don't have to wait and we don't have to pay a fee.
By the way, [/run_operation](https://gitlab.com/tezos/tezos/-/blob/master/docs/api/hangzhou-openapi.json) ignores signature, so we don't even need to forge and sign the operation, just send its content.
 
Let's create a transaction:
 
```cs
var sender   = "tz1ioz62kDw6Gm5HApeQtc1PGmN2wPBtJKUP";
var fa12     = "KT1WUnvcNAnahbuNizZaaAvkpNAEF43KHV7F";
var callback = "KT1DjH58WumESHhpViiwuRE9ehUY5RGMNcuL%viewNat";
            
var rpc = new TezosRpc("https://rpc.tzkt.io/hangzhou2net/");
var counter = await rpc.Blocks.Head.Context.Contracts[sender].Counter.GetAsync<int>();
 
var tx = new TransactionContent
{
    Source = sender,
    Counter = ++counter,
    GasLimit = 100_000,
    Destination = fa12,
    Parameters = new Parameters
    {
        Entrypoint = "getBalance",
        Value = new MichelinePrim
        {
            Prim = PrimType.Pair,
            Args = new List<IMicheline>
            {
                new MichelineString(sender),
                new MichelineString(callback)
            }
        }
    }
};
```
 
Nothing new, this is just a normal transaction with parameters. Then we simulate its execution via RPC:
 
```cs
var operation = await rpc.Blocks.Head.Helpers.Scripts.RunOperation.PostAsync(tx);
```
 
### Extract result
 
And finally, we can parse the simulation result (operation) and, despite it has failed (because the callback contract does nothing but `FAILWITH`), extract the `getBalance` value from the internal transaction parameters:
 
```cs
var balance = operation
    .contents[0]
    .metadata
    .internal_operation_results[0]
    .parameters
    .value
    .@int;
 
// 98770
```
 
That's it. In a similar way we can call any other `view` entrypoints.
 
## Advanced: get FA2 balance
 
Another example, demonstrating more complex `callback` type.
 
### Prepare callback contract
 
In case of FA2 standard, `balance_of` has the following type:
 
```
(pair %balance_of (list %requests (pair (address %owner) (nat %token_id)))
                  (contract %callback (list (pair 
                                              (pair %request (address %owner) (nat %token_id))
                                              (nat %balance)))))
```
 
So we need a callback contract with an entrypoint of type:
 
```
(list (pair 
           (pair %request (address %owner) (nat %token_id))
           (nat %balance)))))
```
 
This time let's originate our own contract that we will use as a `callback` for all calls of FA2 `balance_of`:
 
```cs
var origination = new OriginationContent
{
    Source = sender,
    Counter = ++counter,
    GasLimit = 2000,
    StorageLimit = 100,
    Fee = 1000000,
    Script = new Script
    {
        Code = Micheline.FromJson(@"
            [{""prim"":""parameter"",""args"":[{""prim"":""list"",""args"":[{""prim"":
            ""pair"",""args"":[{""prim"":""pair"",""args"":[{""prim"":""address"",
            ""annots"":[""%owner""]},{""prim"":""nat"",""annots"":[""%token_id""]}],
            ""annots"":[""%request""]},{""prim"":""nat"",""annots"":[""%balance""]}]}]}
            ]},{""prim"":""storage"",""args"":[{""prim"":""unit""}]},{""prim"":""code"",
            ""args"":[[{""prim"":""FAILWITH""}]]}]") as MichelineArray,
        Storage = new MichelinePrim { Prim = PrimType.Unit }
    },
};
 
var bytes = await new LocalForge().ForgeOperationAsync(head, origination);
var sig = (byte[])key.SignOperation(bytes);
 
var op_hash = await rpc.Inject.Operation.PostAsync(bytes.Concat(sig));
// KT1T5ctQSQuoVmfgSaqogxzhKdXs5rCPvKUi
```
 
Now we can use `KT1T5ctQSQuoVmfgSaqogxzhKdXs5rCPvKUi` as a callback contract (in your case it will be a different address).
 
### Simulate operation
 
When we have prepared callback contract, we can simulate an operation:
 
```cs
var fa2 = "KT1X9eKZyo6kQLkJTrjKmVt7MLC33xE6DfZB";
var callback = "KT1T5ctQSQuoVmfgSaqogxzhKdXs5rCPvKUi";
 
var script = await rpc.Blocks.Head.Context.Contracts[fa2].Script.GetAsync();
var code = Micheline.FromJson(script.code);
var cs = new ContractScript(code);
 
var param = cs.BuildParameter(
    "balance_of",
    new
    {
        requests = new []
        {
            new
            {
                owner = sender,
                token_id = 0
            }
        },
        callback
    });
 
var tx = new TransactionContent
{
    Source = key.PubKey.Address,
    Counter = ++counter,
    GasLimit = 100_000,
    StorageLimit = 257,
    Fee = 100_000,
    Destination = fa2,
    Parameters = new Parameters
    {
        Entrypoint = "balance_of",
        Value = param
    }
};
 
var operation = await rpc.Blocks.Head.Helpers.Scripts.RunOperation.PostAsync(tx);
```
 
### Extract result
 
In the same way as with FA1.2 we can extract the value from the simulation result:
 
```cs
var value = operation
    .contents[0]
    .metadata
    .internal_operation_results[0]
    .parameters
    .value;
```
 
This is raw Micheline JSON:
 
```json
[
  {
    "prim":"Pair",
    "args":[
      {
        "prim":"Pair",
        "args":[
          {
            "bytes":"0000fe2ce0cccc0214af521ad60c140c5589b4039247"
          },
          {
            "int":"0"
          }
        ]
      },
      {
        "int":"12345678912"
      }
    ]
  }
]
```
 
We can also convert it to human-readable JSON:
 
```cs
var cbScript = await rpc.Blocks.Head.Context.Contracts[callback].Script.GetAsync();
var cbCode = Micheline.FromJson(cbScript.code);
var cbCs = new ContractScript(cbCode);
 
var json = cbCs.HumanizeParameter("default", Micheline.FromJson(value));
```
 
So in the end we will get:
 
```json
[
  {
    "request":{
      "owner":"tz1ioz62kDw6Gm5HApeQtc1PGmN2wPBtJKUP",
      "token_id":"0"
    },
    "balance":"0"
  }
]
```
 
That's it! :D
