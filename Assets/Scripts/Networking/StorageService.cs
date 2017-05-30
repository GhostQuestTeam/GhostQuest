using GameSparks.Core;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class StorageService
    {
        public void SavePlayer(PlayerGameStats player )
        {

             new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("SAVE_PLAYER")
                .SetEventAttribute("player", player.GSData)
                .Send((response) => {
                if (!response.HasErrors) {
                    Debug.Log("Player Saved To GameSparks...");
                } else {
                    Debug.Log("Error Saving Player Data...");
                }
            });
        }
        
        public void LoadPlayer(PlayerGameStats player )
        {
            new GameSparks.Api.Requests.LogEventRequest().SetEventKey("LOAD_PLAYER").Send((response) => {
                if (!response.HasErrors) {
                    Debug.Log("Received Player Data From GameSparks...");
                    GSData data = response.ScriptData.GetGSData("player_Data");
                } else {
                    Debug.Log("Error Loading Player Data...");
                }
            });
        }
    }
}