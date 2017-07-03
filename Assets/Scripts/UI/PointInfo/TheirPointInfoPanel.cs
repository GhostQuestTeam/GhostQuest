using UnityEngine.UI;

namespace HauntedCity.UI.PointInfo
{
    public class TheirPointInfoPanel:PointInfoPanel
    {
        public Text PointOwner;

        public override void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            base.UpdateView(point);
            PointOwner.text = _point.displayName;
        }

        public void ToFight()
        {
            _gameController.StartBattle(_point);
        }
    }
}