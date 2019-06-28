using System.Collections.Generic;
using Newtonsoft.Json;

namespace Netezos.Rpc
{
    public static class ObjectExtention
    {
        public static string ToJson(this object obj)
        {
            return  JsonConvert.SerializeObject(obj, new JsonSerializerSettings { 
                NullValueHandling = NullValueHandling.Ignore
            });
        }
        public static string ToJsonList(this object obj)
        {
            return  JsonConvert.SerializeObject(new List<object>{obj}, new JsonSerializerSettings { 
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}