using UnityEngine;

namespace HauntedCity.UI.PointInfo
{
    public class PointView:MonoBehaviour
    {
        public ParticleSystem MainPS;
        public ParticleSystem AuxPS;
        public Color YourMainColor;
        public Color TheirMainColor;
        public Color YourAuxColor;
        public Color TheirAuxColor;

        public void UpdateView(bool isYourPoint)
        {
            var mainSettings = MainPS.main;
            var auxSettings = AuxPS.main;
            if (isYourPoint)
            {
                mainSettings.startColor = YourMainColor;
                auxSettings.startColor = YourAuxColor;
            }
            else
            {
                mainSettings.startColor = TheirMainColor;
                auxSettings.startColor = TheirAuxColor;
            }
        }
    }
}