using System;
using HauntedCity.Networking;
using Zenject;

namespace HauntedCity.UI
{
    public class LeaderboardPanel : Panel
    {
        [Inject] private LeaderboardService _leaderboardService;

        public LeaderboardItemView[] Items;
        public LeaderboardItemView UserResult;

        void Start()
        {
            _leaderboardService.OnError += OnError;
            _leaderboardService.OnLoad += OnLeaderboarbLoad;
        }


        protected override void OnShow()
        {
            _leaderboardService.GetLeaderboard();
        }

        private void OnError()
        {
            UserResult.UpdateView(new LeaderboardService.LeaderboardItem("Error"));
        }

        private void OnLeaderboarbLoad(LeaderboardService.LeaderboardItem[] leaderboardItems,
            LeaderboardService.LeaderboardItem userResult
        )
        {
            for (int i = 0; i < Math.Min(leaderboardItems.Length, Items.Length); i++)
            {
                Items[i].UpdateView(leaderboardItems[i]);
            }
            UserResult.UpdateView(userResult);
        }
    }
}