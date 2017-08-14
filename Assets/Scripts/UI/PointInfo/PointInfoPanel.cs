using HauntedCity.GameMechanics.Main;
using HauntedCity.Geo;
using HauntedCity.Networking.Interfaces;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanel:Panel
    {
        protected PointOfInterestData _point;
        
        public Text IncomeLevel;
        public Text DefenceLevel;
        public Text Money;
        public Text Shield;

        [Inject] protected GameController _gameController;
        [Inject] protected IPOIStatsManager _poiStatsManager;

        
        public Text PointInfo;
        public GhostsPanel GhostPanel;

        public virtual void UpdateView(PointOfInterestData point)
        {
            _point = point;
            Model = _point;
            PointInfo.text = _point.LatLon.ToString();
            IncomeLevel.text = _point.Money.Level.ToString();
            DefenceLevel.text = _point.Shield.Level.ToString();
            Money.text = _point.Money.Value + "/" + _point.Money.MaxValue;
            Shield.text = _point.Shield.Value + "/" + _point.Shield.MaxValue;
            GhostPanel.UpdateView(_point);
        }

        public override void UpdateView()
        {
            UpdateView(_point);
        }

        public void Show(PointOfInterestData point)
        {
            UpdateView(point);
            Show();
        }

        

    }
}