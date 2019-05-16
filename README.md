# Netezos

[![Made With](https://img.shields.io/badge/made%20with-C%23-success.svg?)](https://docs.microsoft.com/en-gb/dotnet/csharp/language-reference/)
[![License: MIT](https://img.shields.io/github/license/baking-bad/netezos.svg)](https://opensource.org/licenses/MIT)

.Net Standard 2.0 libraries pack for working with Tezos.

## Netezos.Rpc
Netezos.Rpc provides an access to the Tezos node via RPC API. There is `TezosRpc` class that you should use to build the queries, supported by Tezos RPC API.
### Basic usage
Let's create the instance of `TezosRpc` class, build simple query and execute it by calling `GetAsync()`.

    using (var rpc = new TezosRpc("https://mainnet.tezrpc.me"))
    {
        // get the head block
        var head = await rpc.Blocks.Head.GetAsync();
        
        // get only the hash of the head block
        var hash = await rpc.Blocks.Head.Hash.GetAsync();
    }

Note that a real HTTP request is sent only when you call `GetAsync()`. Until then, you work with just the query object, which can be used to get subqueries.

### Accessing blocks
You can access any block in two ways: by forward indexing and by backward indexing.

    // gets the block with level = 1
    var firstBlock = rpc.Blocks[1];
    
    // gets the last block (e.g. with level = 400000)
    var lastBlock = rpc.Blocks.Head;
    
    // gets the block with level = 400000 - 10 = 399990
    var tenthFromLast = rpc.Blocks[-10];
    
### RpcList and RpcDictionary
The results of many RPC API methods can be interpreted as arrays or dictionaries that allow you to get many objects or only one by specifying a key or index.

    var operations = rpc.Blocks.Head.Operations;
    var firstEndorsement = rpc.Blocks.Head.Operations[0][0];

    var contracts = rpc.Blocks.Head.Context.Contracts;
    var myContract = rpc.Blocks.Head.Context.Contracts["KT1..."];
    
### Query parameters
If some RPC API method has query parameters, the corresponding query object will have the overridden `GetAsync()` methods.

    var activeDelegates = await rpc.Blocks.Head.Context.Delegates.GetAsync(DelegateStatus.Active);

    var bakingRights = await rpc.Blocks.Head.Helpers.BakingRights.GetAsync(maxPriority: 1, all: true);
    
