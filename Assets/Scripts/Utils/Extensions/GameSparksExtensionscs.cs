using GameSparks.Core;

namespace HauntedCity.Utils.Extensions
{
    public static class GameSparksExtensionscs
    {
        public static string GetId(this GSData gsData)
        {
            return gsData.GetGSData("_id").GetString("$oid");
        }
    }
}