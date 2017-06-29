using System;
using GameSparks.Core;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class LeaderboardService
    {
        public event Action<LeaderboardItem[]> OnLoad;
        public event Action OnError;

        public class LeaderboardItem
        {
            public int Place { get; set; }
            public int Points { get; set; }
            public string Name { get; set; }

            public LeaderboardItem(GSData data)
            {
                Name = data.GetString("displayName");
                Points = data.GetInt("numOfPOIs") ?? 0;
            }
        }

        public void GetLeaderboard()
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("GET_LEADERBOARD")
                .Send((response) =>
                {
                    if (!response.HasErrors)
                    {
                        var players = response.ScriptData.GetGSDataList("players");
                        var leaderboardItems = new LeaderboardItem[players.Count];                       
                        for (int i = 0; i < players.Count; i++)
                        {
                            leaderboardItems[i] = new LeaderboardItem(players[i]) {Place = i + 1};
                        }
                        if (OnLoad != null)
                        {
                            OnLoad(leaderboardItems);
                        }
                    }
                    else
                    {
                        Debug.Log("Error on scoreboard load");
                        if (OnError != null)
                        {
                            OnError();
                        }
                    }
                });
        }
    }
}