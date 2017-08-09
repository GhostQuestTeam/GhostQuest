using System;
using GameSparks.Core;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class StorageService
    {
        public PlayerGameStats PlayerStats { get; private set; }
        public event Action OnLoad;
        
        public void LoadPlayer()
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("LOAD_PLAYER")
                .Send((response) => {
                if (!response.HasErrors) {
                    Debug.Log("Received Player Data From GameSparks...");
                    GSData data = response.ScriptData.GetGSData("PLAYER");
                    if (PlayerStats == null)
                    {
                        PlayerStats = new PlayerGameStats();
                    }
                    PlayerStats.GSData = new GSRequestData(data);
                    if (OnLoad != null)
                    {
                        OnLoad();
                    }
                } else {
                    Debug.Log("Error Loading Player Data...");
                }
            });
        }
    }
}