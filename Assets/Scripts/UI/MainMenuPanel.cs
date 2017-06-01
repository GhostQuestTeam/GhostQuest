using HauntedCity.Networking;
using UnityEngine;

namespace HauntedCity.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        public GameObject[] ShowOnlyLoggedUser;
        public GameObject[] ShowOnlyNotLoggedUser;

        private void Start()
        {
            AuthService.Instance.Logout();
        }
        
        private void OnEnable()
        {
            _ShowMenu(AuthService.Instance.IsAuthenticated);
        }

        private void _ShowMenu(bool isAuthenticated)
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
    }
}