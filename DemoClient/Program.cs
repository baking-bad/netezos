using Netezos.Rpc;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;

namespace ConsoleClient
{
    internal sealed class Program
    {
        // For e.g. the public Carthage test net, the test net parameter is false. It is only
        // true for local sandboxed instances of Tezos blockchains
        private static bool useTestNet = false;
        private static readonly TezosRpc Rpc = new TezosRpc(
            ConfigurationManager.AppSettings["Tezos_Node_DaemonUrl_Testnet"],
            useTestNet ? Chain.Test : Chain.Main);

        private static void Main()
        {
            try
            {
                string headBlockHash = Rpc.Blocks.Head.Hash.GetAsync().Result.Value<string>();
                Console.WriteLine($"Head hash is: {headBlockHash}");
            }
            catch (Exception exception)
            {
                Console.WriteLine("[Failed]\n\nPlease check your configuration and make sure that the daemon is up and running and that it is synchronized. \n\nException: " + exception);
            }
        }
    }
}