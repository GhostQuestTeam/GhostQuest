using HauntedCity.Networking;
using UnityEngine;
using Zenject;

namespace HauntedCity.UI
{
    public class MainMenuPanel : Panel
    {
        public GameObject[] ShowOnlyLoggedUser;
        public GameObject[] ShowOnlyNotLoggedUser;

        [Inject] private AuthService _authService;


        private void Start()
        {
        }

        protected override void OnShow()
        {
            Debug.Log("MainMenu OnShow");
            ShowMenu(_authService.IsAuthenticated);
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
            _authService.Logout();
            ShowMenu(_authService.IsAuthenticated);
        }
    }
}