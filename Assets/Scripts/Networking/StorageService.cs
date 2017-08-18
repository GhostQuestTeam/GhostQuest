using System;
using GameSparks.Core;
using HauntedCity.GameMechanics.SkillSystem;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class StorageService
    {
        public event Action<GSData> OnLoad;
        
        public void LoadPlayer()
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("LOAD_PLAYER")
                .Send((response) => {
                if (!response.HasErrors) {
                    Debug.Log("Received Player Data From GameSparks...");
                    GSData data = response.ScriptData.GetGSData("PLAYER");
                   
                    if (OnLoad != null)
                    {
                        OnLoad(data);
                    }
                } else {
                    Debug.Log("Error Loading Player Data...");
                }
            });
        }
    }
}