using System.Runtime.InteropServices;
using HauntedCity.Utils;
using UnityEngine;

namespace HauntedCity.UI
{
    public class Panel : MonoBehaviour
    {
        public void Show()
        {
            transform.localScale = Vector3.one;
            OnShow();
        }

        private Model _model;

        protected virtual Model Model
        {
            get { return _model; }
            set
            {
                if (_model == value) return;
                if (_model != null)
                {
                    _model.OnChange -= UpdateView;
                }
                _model = value;
                if (_model != null)
                {
                    _model.OnChange += UpdateView;
                }
                UpdateView();
            }
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
            UpdateView();
        }

        protected virtual void OnHide()
        {
        }

        public virtual void UpdateView()
        {
        }
    }
}