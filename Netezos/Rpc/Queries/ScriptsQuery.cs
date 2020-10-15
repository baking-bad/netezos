using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    public class ScriptsQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the pack data
        /// </summary>
        public PackDataQuery PackData => new PackDataQuery(this, "pack_data/");

        /// <summary>
        /// Gets the query to the code run
        /// </summary>
        public RunCodeQuery RunCode => new RunCodeQuery(this, "run_code/");

        /// <summary>
        /// Gets the query to the operation run
        /// </summary>
        public RunOperationQuery RunOperation => new RunOperationQuery(this, "run_operation/");

        /// <summary>
        /// Gets the query to the code trace
        /// </summary>
        public TraceCodeQuery TraceCode => new TraceCodeQuery(this, "trace_code/");

        /// <summary>
        /// Gets the query to the code typecheck
        /// </summary>
        public TypeCheckCodeQuery TypeCheckCode => new TypeCheckCodeQuery(this, "typecheck_code/");

        /// <summary>
        /// Gets the query to the data typecheck
        /// </summary>
        public TypeCheckDataQuery TypeCheckData => new TypeCheckDataQuery(this, "typecheck_data/");

        internal ScriptsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}