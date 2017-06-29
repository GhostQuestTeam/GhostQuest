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
        
        public override void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            base.UpdateView(point);
            IncomeLevel.text = _point.incomeLevel.ToString();
            DefenceLevel.text = _point.shieldLevel.ToString();
            Money.text = _point.currentMoney + "/" + _point.MaxMoney;
            Shield.text = _point.currentShield + "/" + _point.MaxShield;
        }
        
        public void UpdateView()
        {
            UpdateView(_point);
        }

        public void UpgradeIncome()
        {;
            _point.TryUpgradeIncome();
            _geoService.UpdatePoint(_point);
            UpdateView();
        }
        
        public void UpgradeShields()
        {
            _point.TryUpgradeShield();
            _geoService.UpdatePoint(_point);
            UpdateView();
        }
        
        public void GetMoney()
        {
            _point.GetMoney();
            _geoService.UpdatePoint(_point);
            UpdateView();
        }
        
        public void RestoreShield()
        {
            _point.TryRestoreShield();
            _geoService.UpdatePoint(_point);
            UpdateView();
        }
    }
}