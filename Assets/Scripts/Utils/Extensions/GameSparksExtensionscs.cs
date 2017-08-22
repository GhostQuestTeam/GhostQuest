using System.Collections.Generic;
using System.Linq;
using GameSparks.Core;

namespace HauntedCity.Utils.Extensions
{
    public static class GameSparksExtensionscs
    {
        public static string GetId(this GSData gsData)
        {
            return gsData.GetGSData("_id").GetString("$oid");
        }

        public static GSRequestData ToGsRequestData(this Dictionary<string, int> dict)
        {
            var result = new GSRequestData();
            foreach (var pair in dict)
            {
                result.AddNumber(pair.Key, pair.Value);
            }
            return result;
        }
        
        public static bool AllZeros(this Dictionary<string, int> dict)
        {
            return (dict.Count == 0) || dict.Values.All((i) => i == 0);
        }
    }
}