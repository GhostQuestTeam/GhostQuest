using System.Collections.Generic;
using System.Linq;
using GameSparks.Core;
using HauntedCity.Networking.Interfaces;
using UnityEngine;

namespace HauntedCity.Networking.GameSparksImpl
{
    public class GameSparksPlayerStatsManager : IPlayerStatsManager
    {
        public void UpgradeAttributes(int survivability, int endurance, int power)
        {
            GSRequestData requestData = new GSRequestData();
            requestData.AddNumber("power", power);
            requestData.AddNumber("survivability", survivability);
            requestData.AddNumber("endurance", endurance);

            Debug.Log(requestData.JSON);
            
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("UPGRADE_PLAYER_STATS")
                .SetEventAttribute("STATS", requestData)
                .Send((response) =>
                {
                    if (response.HasErrors)
                    {
                        Debug.Log("Some erron in UpgradeAttributes");
                    }
                });
        }

        public void BuyWeapon(string weaponId)
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("BUY_WEAPON")
                .SetEventAttribute("WeaponID", weaponId)
                .Send((response) =>
                {
                    if (response.HasErrors)
                    {
                        Debug.Log("Some erron in BuyWeapon");
                    }
                });
            ;
        }

        public void ChooseWeapons(List<string> weaponsIDs)
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("CHOOSE_WEAPON")
                .SetEventAttribute("WEAPONS", weaponsIDs)
                .Send((response) =>
                {
                    if (response.HasErrors)
                    {
                        Debug.Log("Some erron in ChooseWeapons");
                    }
                });
        }
    }
}