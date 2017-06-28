using HauntedCity.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class LeaderboardItemView:MonoBehaviour
    {
        public Text Place;
        public Text Name;
        public Text Points;

        public void UpdateView(LeaderboardService.LeaderboardItem item)
        {
            Place.text = item.Place.ToString();
            Name.text = item.Name;
            Points.text = item.Points.ToString();
        }
    }
}