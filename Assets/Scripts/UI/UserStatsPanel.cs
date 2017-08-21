using System.ComponentModel;
using System.Linq;
using GameSparks.Api.Responses;
using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Networking;
using HauntedCity.UI.PointInfo;
using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class UserStatsPanel:Panel
    {
        public Text Name;
        public Text Health;
        public Text Energy;
        public Text Defence;
        public Text Survivability;
        public Text Power;
        public Text Endurance;

        public GameObject PoiPrefab;
        public Transform PoiContainer;

        [Inject] private BattleStatsCalculator _battleStatsCalculator;
        [Inject] private AuthService _authService;
      
     
        public override void UpdateView()
        {
            var playerStats = GameController.GameStats;
            var battleStats = _battleStatsCalculator.CalculateBattleStats(playerStats);
         
            Name.text = _authService.Nickname;

            Health.Set(battleStats.MaxHealth);
            Energy.Set(battleStats.MaxEnergy);
            Defence.Set(battleStats.Solidity.Defence);
            Power.Set(playerStats.Power);
            Endurance.Set(playerStats.Endurance);
            Survivability.Set(playerStats.Survivability);
            
            PoiContainer.Clear();
            var playerPOIs = playerStats.POIs.Values.ToArray();
            for (var i = 0; i < playerPOIs.Length; i++)
            {
                var poi = playerPOIs[i];
                var poiInfoObject = PoiContainer.AddChild(PoiPrefab);
                poiInfoObject.GetComponent<BriefPointInfo>().UpdateView(poi, i);
            }
        }
    }
}