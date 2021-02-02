### Accessing blocks
You can access any block in two ways: by forward or backward indexing.

```cs
// gets the block with level = 1
var firstBlock = rpc.Blocks[1]; // forward indexing

// gets the last block (e.g. with level = 400000)
var lastBlock = rpc.Blocks.Head;

// gets the block with level = 400000 - 10 = 399990
var tenthFromLast = rpc.Blocks[-10]; // backward indexing
```

### RpcList and RpcDictionary
The results of many RPC API methods can be interpreted as arrays or dictionaries that allow you to get many objects or only one by specifying a key or an index.

```cs
var operations = rpc.Blocks.Head.Operations;
var firstEndorsement = rpc.Blocks.Head.Operations[0][0];

var contracts = rpc.Blocks.Head.Context.Contracts;
var myContract = rpc.Blocks.Head.Context.Contracts["KT1..."];
```

### Query parameters
If some RPC API method has query parameters, the corresponding query object have the overridden `GetAsync()` methods.

```cs
var activeDelegates = await rpc.Blocks.Head.Context.Delegates.GetAsync(DelegateStatus.Active);

var bakingRights = await rpc.Blocks.Head.Helpers.BakingRights.GetAsync(maxPriority: 1, all: true);
```

### POST methods
There are several ways to pass the data to the server.

Some of the RPC queries contain overridden methods which take required and optional parameters. This is enough for the most cases.

```cs
var protoData = await rpc.Blocks.Head.Helpers.Forge.ProtocolData
    .PostAsync(0, "nceUHEeriV43iAfcxsCFf2Ygqn2cQZnuGKump9JEmhaVXt79CvXdY", "0000000349a42671");
```

If the overridden method does not meet your needs, you can simply pass the object with required fields and it will be automatically serialized to JSON before being sent.

```cs
var protoData = await rpc.Blocks.Head.Helpers.Forge.ProtocolData
    .PostAsync(new
    {
        priority = 0,
        nonce_hash = "nceUHEeriV43iAfcxsCFf2Ygqn2cQZnuGKump9JEmhaVXt79CvXdY",
        proof_of_work_nonce = "0000000349a42671"
    });
```

In addition, you can simply pass a valid JSON string.

```cs
var protoData = await rpc.Blocks.Head.Helpers.Forge.ProtocolData
    .PostAsync(@"{""priority"": 0, ""nonce_hash"": ""nceUHEe..."", ""proof_of_work_nonce"":  ""00000...""}");
```