using HauntedCity.GameMechanics.BattleSystem;
using HauntedCity.Geo;
using HauntedCity.Networking.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI.GhostShop
{
    public class GhostCard : Panel
    {
        public Image GhostImage;
        public Text Title;
        public Text Damage;
        public Text Health;
        public Text Shield;
        public Text Speed;
        public Text Price;
        public GameObject SpawnButton;

        public EnemyInfo enemyInfo { get; set; }
        public IPOIStatsManager PoiStatsManager { get; set; }
        private PointOfInterestData _point;

        public virtual void UpdateView(PointOfInterestData point)
        {
            _point = point;
            Model = _point;
        }

        public override void UpdateView()
        {

            Title.text = enemyInfo.Id;
            if (_point.Enemies.ContainsKey(enemyInfo.Id))
            {
                 Title.text +=   " (" + _point.Enemies[enemyInfo.Id] + ")";
            }
            else
            {
                Title.text += "(0)";
            }
            Damage.text = "Damage: " + enemyInfo.Damage;
            Health.text = "Energy cost: " + enemyInfo.MaxHealth;
            Shield.text = "Defence: " + enemyInfo.Defence;
            Speed.text = "Speed: " + enemyInfo.Velocity;
            Price.text = enemyInfo.Price.ToString();
            SpawnButton.SetActive(_point.CanSpawnGhost(enemyInfo));
        }

        public void SpawnGhost()
        {
            if (_point.TrySpawnGhost(enemyInfo))
            {
                PoiStatsManager.SpawnGhost(_point.Poid, enemyInfo.Id);
            }
            
        }
    }
}