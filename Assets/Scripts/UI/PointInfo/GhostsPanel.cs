using HauntedCity.Geo;
using HauntedCity.Utils.Extensions;
using UnityEngine;

namespace HauntedCity.UI.PointInfo
{
    public class GhostsPanel:Panel
    {
        public GameObject GhostInfoPrefab;
        public Transform GhostContainer;

        public void UpdateView(PointOfInterestData point)
        {
            GhostContainer.Clear();
            foreach (var enemy in point.Enemies)
            {
                var ghostView = Instantiate(GhostInfoPrefab);
                
                ghostView.transform.SetParent(GhostContainer, false);
                ghostView.GetComponent<GhostView>().UpdateView(enemy.Key, enemy.Value);
            }
        }
    }
}