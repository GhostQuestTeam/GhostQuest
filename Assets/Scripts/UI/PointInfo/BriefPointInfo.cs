using HauntedCity.Geo;
using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI.PointInfo
{
    public class BriefPointInfo:MonoBehaviour
    {
        public Text Position;
        public Text ShieldLevel;
        public Text IncomeLevel;
        public Text Money;
        public Text Shields;
        public Text Lat;
        public Text Lon;

        public void UpdateView(PointOfInterestData point, int position)
        {
            Position.text = "#" + position;
            ShieldLevel.Set(point.Shield.Level);
            IncomeLevel.Set(point.Money.Level);
            Money.Set(point.Money.Value);
            Shields.Set(point.Shield.Value);
            Lat.Set(point.LatLon.x);
            Lon.Set(point.LatLon.y);
        }
    }
}