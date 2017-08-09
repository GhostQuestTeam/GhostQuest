using HauntedCity.Geo;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI.PointInfo
{
    public class YourPointInfoPanel:PointInfoPanel
    {
        public Text IncomeLevel;
        public Text DefenceLevel;
        public Text Money;
        public Text Shield;

        private GameSparksPOIsExtraction _geoService;
        
        void Awake()
        {
            _geoService = GameObject.FindObjectOfType<GameSparksPOIsExtraction>();
        }
        
        public override void UpdateView(PointOfInterestData point)
        {
            base.UpdateView(point);
            IncomeLevel.text = _point.Money.Level.ToString();
            DefenceLevel.text = _point.Shield.Level.ToString();
            Money.text = _point.Money.Value + "/" + _point.Money.MaxValue;
            Shield.text = _point.Shield.Value + "/" + _point.Shield.MaxValue;
        }
        
        public void UpdateView()
        {
            UpdateView(_point);
        }

        public void UpgradeIncome()
        {
            _point.Money.TryUpgrade();
            UpdateView();
        }
        
        public void UpgradeShields()
        {
            _point.Shield.TryUpgrade();
            UpdateView();
        }
        
        public void GetMoney()
        {
            _point.Money.TakeMoney();
            UpdateView();
        }
        
        public void RestoreShield()
        {
            _point.Shield.TryRestore();
            UpdateView();
        }
    }
}