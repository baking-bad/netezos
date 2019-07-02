using System.Collections.Generic;
 using System.Threading.Tasks;
 using Newtonsoft.Json.Linq;
  
 namespace Netezos.Rpc.Queries.Post
 {
     public class InjectBlockQuery : RpcPost
     {
         internal InjectBlockQuery(RpcQuery baseQuery, string append) : base(baseQuery, append)
         {
         }
         /// <summary>Inject block query</summary>
         /// <param name="data">Data</param>
         /// <param name="operations">List of operations</param>
         /// <param name="async">Async</param>
         /// <param name="force">Force</param>
         /// <param name="chain">Chain</param>
         /// <returns></returns>
         public async Task<JToken> PostAsync(string data, List<List<object>> operations, bool async = false, bool force = false, string chain = "main")
             => await Client.Post(
                 $"{Query}?async={async}&force={force}&chain={chain}",
                 new
                 {
                     data,
                     operations
                 }.ToJson());
     }
 }