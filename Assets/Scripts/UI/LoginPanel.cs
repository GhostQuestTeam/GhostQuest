using GameSparks.Api.Responses;
using HauntedCity.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class LoginPanel : MonoBehaviour
    {
        public Animator MainMenu;
        
        private InputField _login;
        private InputField _password;
        
        private ScreenManager _screenManager;
        
        [Inject]
        public void InitializeDependencies(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        private void Start()
        {
            _login = transform.Find("LoginForm/Login").GetComponent<InputField>();
            _password = transform.Find("LoginForm/Password").GetComponent<InputField>();
        }

        public void Login()
        {
            AuthService.Instance.Login(
                _login.text,
                _password.text
            );
        }

        private void OnEnable()
        {
            AuthService.Instance.OnLogin += OnLogin;
        }
        
        private void OnDisable()
        {
            AuthService.Instance.OnLogin -= OnLogin;
        }
        
        public void OnLogin(AuthenticationResponse response)
        {
            if (!response.HasErrors)
            {
                _screenManager.OpenPanel(MainMenu);
            }
            else
            {
                //TODO
            }
        }

        public void Logout()
        {
            AuthService.Instance.Logout();
        }
    }
}