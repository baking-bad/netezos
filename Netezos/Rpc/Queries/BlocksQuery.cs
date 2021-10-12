using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Rpc query to access blocks data
    /// </summary>
    public class BlocksQuery : RpcObject
    {
        /// <summary>
        /// Gets the query to the current head
        /// </summary>
        public BlockQuery Head => new BlockQuery(this, "head/");

        /// <summary>
        /// Gets the query to the block at the specified level
        /// </summary>
        /// <param name="level">Level of the block. If level < 0, then it will be used as [head - level].</param>
        /// <returns></returns>
        public BlockQuery this[int level]
            => level >= 0 ? new BlockQuery(this, $"{level}/") : new BlockQuery(this, $"head~{-level}/");

        /// <summary>
        /// Gets the query to the block with the specified hash
        /// </summary>
        /// <param name="hash">Hash of the block</param>
        /// <returns></returns>
        public BlockQuery this[string hash] => new BlockQuery(this, $"{hash}/");

        internal BlocksQuery(RpcClient client, string query) : base(client, query) { }

        /// <summary>
        /// Executes the query and returns known heads of the blockchain, sorted with decreasing fitness
        /// </summary>
        /// <returns></returns>
        public new Task<dynamic> GetAsync()
            => Client.GetJson(Query);

        /// <summary>
        /// Executes the query and returns known heads of the blockchain, sorted with decreasing fitness
        /// </summary>
        /// <param name="length">Number of blocks per head (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<dynamic> GetAsync(int length)
            => Client.GetJson($"{Query}?length={length}");

        /// <summary>
        /// Executes the query and returns known heads of the blockchain, sorted with decreasing fitness
        /// </summary>
        /// <param name="minDate">Datetime before which the heads are filtered out</param>
        /// <param name="length">Number of blocks per head (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<dynamic> GetAsync(DateTime minDate, int length = 1)
            => Client.GetJson($"{Query}?min_date={minDate.ToUnixTime()}&length={length}");

        /// <summary>
        /// Executes the query and returns the fragment of the chain before the specified block
        /// </summary>
        /// <param name="head">Hash of the block which is considered head</param>
        /// <param name="length">Number of blocks (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<dynamic> GetAsync(string head, int length = 1)
            => Client.GetJson($"{Query}?head={head}&length={length}");

        /// <summary>
        /// Executes the query and returns the fragments of the chain before the specified blocks
        /// </summary>
        /// <param name="heads">Hashes of the blocks which are considered heads</param>
        /// <param name="length">Number of blocks per head (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<dynamic> GetAsync(List<string> heads, int length = 1)
            => Client.GetJson($"{Query}?{String.Join("&", heads.Select(h => $"head={h}"))}&length={length}");

        /// <summary>
        /// Executes the query and returns known heads of the blockchain, sorted with decreasing fitness
        /// </summary>
        /// <returns></returns>
        public new Task<T> GetAsync<T>()
            => Client.GetJson<T>(Query);

        /// <summary>
        /// Executes the query and returns known heads of the blockchain, sorted with decreasing fitness
        /// </summary>
        /// <param name="length">Number of blocks per head (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(int length)
            => Client.GetJson<T>($"{Query}?length={length}");

        /// <summary>
        /// Executes the query and returns known heads of the blockchain, sorted with decreasing fitness
        /// </summary>
        /// <param name="minDate">Datetime before which the heads are filtered out</param>
        /// <param name="length">Number of blocks per head (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(DateTime minDate, int length = 1)
            => Client.GetJson<T>($"{Query}?min_date={minDate.ToUnixTime()}&length={length}");

        /// <summary>
        /// Executes the query and returns the fragment of the chain before the specified block
        /// </summary>
        /// <param name="head">Hash of the block which is considered head</param>
        /// <param name="length">Number of blocks (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(string head, int length = 1)
            => Client.GetJson<T>($"{Query}?head={head}&length={length}");

        /// <summary>
        /// Executes the query and returns the fragments of the chain before the specified blocks
        /// </summary>
        /// <param name="heads">Hashes of the blocks which are considered heads</param>
        /// <param name="length">Number of blocks per head (head + predessors) to returns</param>
        /// <returns></returns>
        public Task<T> GetAsync<T>(List<string> heads, int length = 1)
            => Client.GetJson<T>($"{Query}?{String.Join("&", heads.Select(h => $"head={h}"))}&length={length}");
    }
}
