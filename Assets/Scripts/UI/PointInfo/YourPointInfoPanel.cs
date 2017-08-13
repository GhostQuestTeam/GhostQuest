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
        public Text ShieldUpgradePrice;
        public Text IncomeUpgradePrice;

        public GameObject ShieldUpgradeButton;
        public GameObject IncomeUpgradeButton;
        public GameObject TakeMoneyButton;
        
        
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
           
            ShieldUpgradeButton.SetActive(_point.Shield.CanUpgrade());
            IncomeUpgradeButton.SetActive(_point.Money.CanUpgrade());
//            TakeMoneyButton.SetActive(_point.Money.CanTakeMoney());
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