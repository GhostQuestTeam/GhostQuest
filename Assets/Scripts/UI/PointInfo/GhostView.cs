using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI.PointInfo
{
    public class GhostView:MonoBehaviour
    {
        public Text Value;
        public Image Picture;

        public void UpdateView(string ghostType, int ghostCount)
        {
            Value.text = ghostType + " : " + ghostCount;
        }
    }
}