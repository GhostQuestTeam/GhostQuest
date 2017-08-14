using System.Collections.Generic;
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
            UpdateView(point.Enemies);
        }

        public void UpdateView(Dictionary<string, int> enemies)
        {
            GhostContainer.Clear();
            foreach (var enemy in enemies)
            {
                if(enemy.Value == 0) continue;
                var ghostView = Instantiate(GhostInfoPrefab);
                
                ghostView.transform.SetParent(GhostContainer, false);
                ghostView.GetComponent<GhostView>().UpdateView(enemy.Key, enemy.Value);
            }
        }
    }
}