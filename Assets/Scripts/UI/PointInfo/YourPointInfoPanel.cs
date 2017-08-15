using HauntedCity.Geo;
using HauntedCity.Networking.Interfaces;
using HauntedCity.UI.GhostShop;
using HauntedCity.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class YourPointInfoPanel : PointInfoPanel
    {
        
        public Text ShieldUpgradePrice;
        public Text IncomeUpgradePrice;
        public Text RestoreShieldPrice;

        public GameObject ShieldUpgradeButton;
        public GameObject IncomeUpgradeButton;
        public GameObject TakeMoneyButton;

        public GhostShopPanel GhostShop;

        void Awake()
        {
        }

        public override void UpdateView(PointOfInterestData point)
        {
            base.UpdateView(point);
            
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

        public void ToGhostShop()
        {
            GhostShop.UpdateView(_point);
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