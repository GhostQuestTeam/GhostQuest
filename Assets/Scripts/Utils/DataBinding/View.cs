using UnityEngine;

namespace HauntedCity.Utils.DataBinding
{
    public class View:MonoBehaviour
    {
        protected virtual Model GetModel()
        {
            return null;
        }

        void OnEnable()
        {
            if (GetModel() != null)
            {
                GetModel().OnChange += UpdateView;
            }
        }

        private void OnDisable()
        {
            if (GetModel() != null)
            {
                GetModel().OnChange -= UpdateView;
            }
        }

        public virtual void UpdateView()
        {
            
        }
    }
}