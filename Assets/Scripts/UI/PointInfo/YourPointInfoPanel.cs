using UnityEngine.UI;

namespace HauntedCity.UI.PointInfo
{
    public class YourPointInfoPanel:PointInfoPanel
    {
        public Text IncomeLevel;
        public Text DefenceLevel;
        public Text Money;
        public Text Defence;
        public override void UpdateView(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            base.UpdateView(point);
        }
    }
}