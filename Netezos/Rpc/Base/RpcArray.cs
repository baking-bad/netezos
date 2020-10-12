namespace Netezos.Rpc
{
    /// <summary>
    /// Rpc query to get an array of json objects, which can also be accessed by index
    /// </summary>
    /// <typeparam name="T">Type of the objects in the array</typeparam>
    public class RpcArray<T> : RpcObject where T : RpcObject
    {
        #region static
        static Creator<T> _CreateRpcObject = null;
        static Creator<T> CreateRpcObject
        {
            get
            {
                if (_CreateRpcObject == null)
                    _CreateRpcObject = GetCreator<T>();

                return _CreateRpcObject;
            }
        }
        #endregion

        /// <summary>
        /// Gets the query to object at the specified index in the array
        /// </summary>
        /// <param name="index">Zero-based index of the object to query</param>
        /// <returns></returns>
        public T this[int index] => CreateRpcObject(this, $"{index}/");
        
        internal RpcArray(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
