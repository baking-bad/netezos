namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to get an array of json objects, which can also be accessed by index
    /// </summary>
    /// <typeparam name="T">Type of the objects in the array</typeparam>
    public class RpcArray<T> : RpcObject where T : RpcObject
    {
        #region static
        static Creator<T>? _CreateRpcObject = null;
        static Creator<T> CreateRpcObject => _CreateRpcObject ??= GetCreator<T>();
        #endregion

        /// <summary>
        /// Gets the query to object at the specified index in the array
        /// </summary>
        /// <param name="index">Zero-based index of the object to query</param>
        /// <returns></returns>
        public T this[int index] => CreateRpcObject(this, $"{index}/");
        
        internal RpcArray(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }

    /// <summary>
    /// Rpc query to get json objects by id or index
    /// </summary>
    /// <typeparam name="T">Type of the objects in the array</typeparam>
    public class RpcSimpleArray<T> : RpcQuery where T : RpcQuery
    {
        #region static
        static RpcObject.Creator<T>? _CreateRpcObject = null;
        static RpcObject.Creator<T> CreateRpcObject => _CreateRpcObject ??= RpcObject.GetCreator<T>();
        #endregion

        /// <summary>
        /// Gets the query to object at the specified index in the array
        /// </summary>
        /// <param name="index">Zero-based index of the object to query</param>
        /// <returns></returns>
        public T this[int index] => CreateRpcObject(this, $"{index}/");

        internal RpcSimpleArray(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
