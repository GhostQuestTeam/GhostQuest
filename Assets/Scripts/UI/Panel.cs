using GameSparks.Api.Responses;
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
        
        public void Hide()
        {
            transform.localScale = Vector3.zero;
            OnHide();
        }
        
        protected virtual void OnShow(){}
        
        protected virtual void OnHide(){}
    }
}