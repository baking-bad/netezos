namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to get a dictionary of json objects, which can also be accessed by key
    /// </summary>
    /// <typeparam name="TKey">Type of the keys to access the objects in the dictionary</typeparam>
    /// <typeparam name="TValue">Type of the objects in the dictionary</typeparam>
    public class RpcDictionary<TKey, TValue> : RpcObject where TValue : RpcObject
    {
        #region static
        static Creator<TValue> _CreateRpcObject = null;
        static Creator<TValue> CreateRpcObject
        {
            get
            {
                if (_CreateRpcObject == null)
                    _CreateRpcObject = GetCreator<TValue>();

                return _CreateRpcObject;
            }
        }
        #endregion

        /// <summary>
        /// Gets the query to object associated with the specified key
        /// </summary>
        /// <param name="key">Key of the object to query</param>
        /// <returns></returns>
        public TValue this[TKey key] => CreateRpcObject(this, $"{key}/");

        internal RpcDictionary(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }

    /// <summary>
    /// Rpc query to get json objects by key
    /// </summary>
    /// <typeparam name="TKey">Type of the keys to access the objects in the dictionary</typeparam>
    /// <typeparam name="TValue">Type of the objects in the dictionary</typeparam>
    public class RpcSimpleDictionary<TKey, TValue> : RpcQuery where TValue : RpcQuery
    {
        #region static
        static RpcObject.Creator<TValue> _CreateRpcObject = null;
        static RpcObject.Creator<TValue> CreateRpcObject
        {
            get
            {
                if (_CreateRpcObject == null)
                    _CreateRpcObject = RpcObject.GetCreator<TValue>();

                return _CreateRpcObject;
            }
        }
        #endregion

        /// <summary>
        /// Gets the query to object associated with the specified key
        /// </summary>
        /// <param name="key">Key of the object to query</param>
        /// <returns></returns>
        public TValue this[TKey key] => CreateRpcObject(this, $"{key}");

        internal RpcSimpleDictionary(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
