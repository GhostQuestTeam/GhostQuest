using HauntedCity.Geo;
using HauntedCity.Utils.Extensions;
using UnityEngine.UI;
using UnityEngine;

namespace HauntedCity.UI.PointInfo
{
    public class TheirPointInfoPanel:PointInfoPanel
    {
        public Text PointOwner;
        public GameObject FightButton;
        public GameObject AttackButton;
        public Text AttackPrice;
        
        private void Start()
        {
            AttackPrice.text = POIShield.ATTACK_PRICE.ToString();
        }

        public override void UpdateView(PointOfInterestData point)
        {
            base.UpdateView(point);
            PointOwner.text = _point.DisplayName;

            var canFight = _point.Shield.Value == 0;
            FightButton.SetActive(canFight);
            AttackButton.SetActive(!canFight);
        }

        private GameSparksBattle _gsb;

        public void OnStartCapture(object sender, GameSparksBattle.POI_START_CAP_ev_arg arg)
        {
            if (arg.poid == _point.Poid)
            {
                //_gsb.OnPOIStartCap -= OnStartCapture;
                if (arg.isError || !arg.isStarted)
                {
                    //WE DID NOT START - TODO SHOW ERROR
                }
                else
                {
                    Hide();
                    if (!_point.Enemies.AllZeros())
                    {
                        _gameController.StartBattle(_point);
                    }
                }
                
            }
        }

        public void ToFight()
        {
            if (_gsb == null)
            {
                _gsb = GameObject.Find("GameSparks").GetComponent<GameSparksBattle>();
                _gsb.OnPOIStartCap += OnStartCapture;
            }
            _gsb.sendStartCapture(_point.Poid);
            
        }

        public void Attack()
        {
            if (_point.Shield.CanAttack())
            {
                _point.AttackShield();
                _poiStatsManager.AttackShield(_point.Poid);
            }
        }

        public void OnDestroy()
        {
            _gsb.OnPOIStartCap -= OnStartCapture;
        }
    }
}