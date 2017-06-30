using HauntedCity.Networking;
using UnityEngine;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanelController:MonoBehaviour
    {
        public YourPointInfoPanel yourPointrPanelTemplate;
        public TheirPointInfoPanel theirPointPanelTemplate;

        public void Show(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            if (point.displayName == AuthService.Instance.Nickname)
            {
                yourPointrPanelTemplate.Show(point);
            }
            else
            {
                theirPointPanelTemplate.Show(point);
            }
        }
    }
}