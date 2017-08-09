using HauntedCity.Utils;
using UnityEngine;

namespace HauntedCity.UI
{
    public class Panel:MonoBehaviour
    {   
        public void Show()
        {
            transform.localScale = Vector3.one;
            OnShow();
        }
        
        protected virtual Model GetModel()
        {
            return null;
        }

        public void ShowInstead(Panel other)
        {
            Hide();
            other.Show();
        }
        
        public void Hide()
        {
            transform.localScale = Vector3.zero;
            OnHide();
        }

        protected virtual void OnShow()
        {
            if (GetModel() != null)
            {
                GetModel().OnChange += UpdateView;
            }
            UpdateView();
        }

        protected virtual void OnHide()
        {
            if (GetModel() != null)
            {
                GetModel().OnChange += UpdateView;
            }
        }
        
        public virtual void UpdateView(){} 
    }
}