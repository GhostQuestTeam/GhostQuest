using System;
using GameSparks.Core;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class LeaderboardService
    {
        public event Action<LeaderboardItem[], LeaderboardItem> OnLoad;
        public event Action OnError;

        public class LeaderboardItem
        {
            public int Place { get; set; }
            public int Points { get; set; }
            public string Name { get; set; }

            public LeaderboardItem(GSData data)
            {
                if (data == null)
                {
                    Name = AuthService.Instance.Nickname;
                    Place = Points = 0;
                    return;
                }
                Name = data.GetString("displayName");
                Points = data.GetInt("numOfPOIs") ?? 0;
                Place = data.GetInt("place") ?? 0;
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
                        var user = response.ScriptData.GetGSData("rank");
                        var userResult = new LeaderboardItem(user);
                        var players = response.ScriptData.GetGSDataList("players");
                        var leaderboardItems = new LeaderboardItem[players.Count];
                        var currentPlace = 1;
                        var currentMax = players[0].GetInt("numOfPOIs");
                        for (int i = 0; i < players.Count; i++)
                        {
                            leaderboardItems[i] = new LeaderboardItem(players[i]) ;
                            if (leaderboardItems[i].Points < currentMax)
                            {
                                currentMax = leaderboardItems[i].Points;
                                currentPlace++;
                            }
                            leaderboardItems[i].Place = currentPlace;
                        }
                        if (OnLoad != null)
                        {
                            OnLoad(leaderboardItems, userResult);
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