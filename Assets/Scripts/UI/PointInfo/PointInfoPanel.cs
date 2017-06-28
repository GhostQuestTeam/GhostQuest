using HauntedCity.GameMechanics.Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanel:Panel
    {
        private GameSparksPOIsExtraction.ExtractedPointMetadata _point;

        [Inject] private GameController _gameController;
        
        public Text PointOwner;
        public Text PointInfo;
        public GhostsPanel GhostPanel;

        public void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            _point = point;
            PointOwner.text = _point.uoid;
            PointInfo.text = _point.LatLon.ToString();
            GhostPanel.UpdateView(_point);
        }

        public void Show(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            UpdateView(point);
            Show();
        }

        public void ToFight()
        {
            _gameController.StartBattle(_point);
        }

    }
}