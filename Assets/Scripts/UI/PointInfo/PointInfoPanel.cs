using HauntedCity.GameMechanics.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanel:Panel
    {
        protected GameSparksPOIsExtraction.ExtractedPointMetadata _point;

        [Inject] protected GameController _gameController;
        
        public Text PointInfo;
        public GhostsPanel GhostPanel;

        public virtual void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            _point = point;
            PointInfo.text = _point.LatLon.ToString();
            GhostPanel.UpdateView(_point);
        }

        public void Show(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            UpdateView(point);
            Show();
        }

        

    }
}