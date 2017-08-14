using HauntedCity.GameMechanics.Main;
using HauntedCity.Geo;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanel:Panel
    {
        protected PointOfInterestData _point;

        [Inject] protected GameController _gameController;
        
        public Text PointInfo;
        public GhostsPanel GhostPanel;

        public virtual void UpdateView(PointOfInterestData point)
        {
            _point = point;
            Model = _point;
            PointInfo.text = _point.LatLon.ToString();
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