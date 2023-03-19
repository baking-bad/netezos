using Netezos.Rpc.Queries.Post;

namespace Netezos.Rpc.Queries
{
    public class ScriptsQuery : RpcQuery
    {
        /// <summary>
        /// Gets the query to the pack data
        /// </summary>
        public PackDataQuery PackData => new(this, "pack_data/");

        /// <summary>
        /// Gets the query to the code run
        /// </summary>
        public RunCodeQuery RunCode => new(this, "run_code/");

        /// <summary>
        /// Gets the query to the operation run
        /// </summary>
        public RunOperationQuery RunOperation => new(this, "run_operation/");
        
        /// <summary>
        /// Gets the query to the script view run
        /// </summary>
        public RunScriptViewQuery RunScriptView => new(this, "run_script_view/");

        /// <summary>
        /// Gets the query to the operation simulation
        /// </summary>
        public SimulateOperationQuery SimulateOperation => new(this, "simulate_operation/");

        /// <summary>
        /// Gets the query to the code trace
        /// </summary>
        public TraceCodeQuery TraceCode => new(this, "trace_code/");

        /// <summary>
        /// Gets the query to the code typecheck
        /// </summary>
        public TypeCheckCodeQuery TypeCheckCode => new(this, "typecheck_code/");

        /// <summary>
        /// Gets the query to the data typecheck
        /// </summary>
        public TypeCheckDataQuery TypeCheckData => new(this, "typecheck_data/");

        internal ScriptsQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}