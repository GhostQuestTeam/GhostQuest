using HauntedCity.Networking;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        public GameObject[] ShowOnlyLoggedUser;
        public GameObject[] ShowOnlyNotLoggedUser;

       
        private void Start()
        {
        }
        
        private void OnEnable()
        {
            ShowMenu(AuthService.Instance.IsAuthenticated);
        }

        public void ShowMenu(bool isAuthenticated)
        {
            foreach (var item in ShowOnlyLoggedUser)
            {
                item.SetActive(isAuthenticated);
            }
            foreach (var item in ShowOnlyNotLoggedUser)
            {
                item.SetActive(!isAuthenticated);
            }
        }

        public void Logout()
        {
            AuthService.Instance.Logout();
            ShowMenu(AuthService.Instance.IsAuthenticated);
        }
    }
}