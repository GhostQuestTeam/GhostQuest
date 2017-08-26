using System.Linq;
using Facebook.Unity;
using GooglePlayGames;
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
            #if !UNITY_EDITOR
            GameObject.FindGameObjectsWithTag("OnlyEditor").ToList().ForEach((obj)=> obj.SetActive(false));
            #endif
            if (FB.IsLoggedIn)
            {
                _authService.SocialAuth(AuthService.AuthType.Facebook);
            }
//            if (PlayGamesPlatform.Instance.IsAuthenticated())
//            {
//                _authService.SocialAuth(AuthService.AuthType.GooglePlus);
//            }
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