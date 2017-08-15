using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.Geo;
using HauntedCity.Networking.Interfaces;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI.GhostShop
{
    public class GhostShopPanel:Panel
    {
        public GameObject GhostCardPrefab;
        public Transform CardContainer;

        private EnemyInfo[] _allowableGhosts;
        private GhostCard[] _ghostCards;
        private PointOfInterestData _point;
        [Inject] private IPOIStatsManager _poiStatsManager;
        
        private void Awake()
        {
            _allowableGhosts = Resources.LoadAll<EnemyInfo>("");
            Draw();

        }

        private void OnEnable()
        {
            foreach (var ghostCard in _ghostCards)
            {
                ghostCard.PoiStatsManager = _poiStatsManager;
            }
        }

        public void Draw()
        {
            _ghostCards = new GhostCard[_allowableGhosts.Length];
            for (int i = 0; i < _ghostCards.Length; i++)
            {
                var ghostCard = Instantiate(GhostCardPrefab);
                ghostCard.transform.SetParent(CardContainer, false);

                var ghostCardComponent = ghostCard.GetComponent<GhostCard>();
                ghostCardComponent.enemyInfo = _allowableGhosts[i];
                _ghostCards[i] = ghostCardComponent;
            }
        }
        
        
        public void UpdateView(PointOfInterestData point)
        {
            _point = point;
            foreach (var ghostCard in _ghostCards)
            {
                ghostCard.UpdateView(_point);
            }
            Show();
        }

       
    }
}