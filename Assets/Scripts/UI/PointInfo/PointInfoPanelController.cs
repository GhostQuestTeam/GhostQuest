using HauntedCity.Geo;
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
        

        public void Show(PointOfInterestData point)
        {
            if (point.DisplayName == _authService.Nickname && point.DisplayName != "")
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