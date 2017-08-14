using HauntedCity.Geo;
using HauntedCity.Networking.Interfaces;
using HauntedCity.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class YourPointInfoPanel : PointInfoPanel
    {
        public Text IncomeLevel;
        public Text DefenceLevel;
        public Text Money;
        public Text Shield;
        public Text ShieldUpgradePrice;
        public Text IncomeUpgradePrice;
        public Text RestoreShieldPrice;

        public GameObject ShieldUpgradeButton;
        public GameObject IncomeUpgradeButton;
        public GameObject TakeMoneyButton;

        [Inject] private IPOIStatsManager _poiStatsManager;

        void Awake()
        {
        }

        public override void UpdateView(PointOfInterestData point)
        {
            base.UpdateView(point);
            IncomeLevel.text = _point.Money.Level.ToString();
            DefenceLevel.text = _point.Shield.Level.ToString();
            Money.text = _point.Money.Value + "/" + _point.Money.MaxValue;
            Shield.text = _point.Shield.Value + "/" + _point.Shield.MaxValue;
            ShieldUpgradePrice.text = _point.Shield.Price.ToString();
            IncomeUpgradePrice.text = _point.Money.Price.ToString();
            RestoreShieldPrice.text = "(" + POIShield.RESTORE_PRICE + ")";
            
            ShieldUpgradeButton.SetActive(_point.Shield.CanUpgrade());
            IncomeUpgradeButton.SetActive(_point.Money.CanUpgrade());
//            TakeMoneyButton.SetActive(_point.Money.CanTakeMoney());
        }

  
        public void UpgradeIncome()
        {
            if (_point.Money.CanUpgrade())
            {
                _point.UpgradeIncome();
                _poiStatsManager.UpgradeIncome(_point.Poid);
            }
        }

        public void UpgradeShields()
        {
            if (_point.Shield.CanUpgrade())
            {
                _point.UpgradeShield();
                _poiStatsManager.UpgradeShield(_point.Poid);
            }
        }

        public void GetMoney()
        {
            _point.Money.TakeMoney();
            _poiStatsManager.TakeMoney(_point.Poid);
        }

        public void RestoreShield()
        {
            if (_point.Shield.CanRestore())
            {
                _point.RestoreShield();
                _poiStatsManager.RestoreShield(_point.Poid);
            }
        }
    }
}