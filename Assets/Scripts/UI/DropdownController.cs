using ModestTree;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class DropdownController:MonoBehaviour
    {
        public Button ShowButton;
        public Button HideButton;
        public GameObject[] MenuElements;


        private void OnEnable()
        {
            ShowButton.onClick.AddListener(Show);
            HideButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            ShowButton.onClick.RemoveListener(Show);
            HideButton.onClick.RemoveListener(Hide);
        }

        private void ChangeState(bool menuOpened)
        {
            ShowButton.gameObject.SetActive(!menuOpened);
            HideButton.gameObject.SetActive(menuOpened);
            MenuElements.ForEach((obj) => obj.SetActive(menuOpened));
        }
        
        public void Show()
        {
            ChangeState(true);
        }

        public void Hide()
        {
            ChangeState(false);
        }
        
    }
}