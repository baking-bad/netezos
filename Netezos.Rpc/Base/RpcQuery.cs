﻿namespace Netezos.Rpc
{
    /// <summary>
    /// Base class for RPC queries
    /// </summary>
    public class RpcQuery
    {
        internal RpcQuery Base;
        internal RpcClient Client;
        internal string Query;

        internal RpcQuery(RpcClient client, string query)
        {
            Client = client;
            Query = query;
        }

        internal RpcQuery(RpcQuery baseQuery, string append)
        {
            Base = baseQuery;
            Client = baseQuery.Client;
            Query = baseQuery.Query + append;
        }

        internal RpcQuery(RpcQuery baseQuery)
        {
            Base = baseQuery;
            Client = baseQuery.Client;
            Query = baseQuery.Query;
        }

        public override string ToString() => Query;
    }
}
