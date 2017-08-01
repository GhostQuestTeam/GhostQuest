using HauntedCity.Networking;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI.PointInfo
{
    public class PointInfoPanelController:MonoBehaviour
    {
        [Inject] private AuthService _authService;
        
        public YourPointInfoPanel yourPointrPanelTemplate;
        public TheirPointInfoPanel theirPointPanelTemplate;
        

        public void Show(GameSparksPOIsExtraction.ExtractedPointMetadata point)
        {
            if (point.displayName == _authService.Nickname && point.displayName != "")
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