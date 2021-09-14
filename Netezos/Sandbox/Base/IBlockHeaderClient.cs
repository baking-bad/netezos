
using Netezos.Sandbox.HeaderMethods;

namespace Netezos.Sandbox
{
    public interface IBlockHeaderClient
    {
        /// <summary>
        /// Create call to bake genesis block with specified parameters
        /// </summary>
        /// <param name="keyName">alias key</param>
        /// <returns></returns>
        ActivateProtocolMethodHandler ActivateProtocol(string keyName);

        /// <summary>
        /// Create call to bake new block
        /// <param name="keyName">alias key</param>
        /// <param name="minFee">min fee</param>
        /// </summary>
        BakeBlockMethodHandler BakeBlock(string keyName, int minFee = 0);
    }
}