using System.Threading.Tasks;

namespace Netezos.Rpc.Queries
{
    /// <summary>
    /// Liquidity baking CPMM address
    /// </summary>
    public class LiquidityBakingQuery : RpcObject
    {
        public RpcObject CpmmAddress => new RpcObject(this, "cpmm_address/");
        
        internal LiquidityBakingQuery(RpcQuery baseQuery, string append) : base(baseQuery, append) { }
    }
}
