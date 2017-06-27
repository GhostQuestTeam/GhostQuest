using GameSparks.Api.Responses;
using HauntedCity.Networking;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class RegistrationPanel : Panel
    {
        public Panel MainMenu;
        
        private InputField _login;
        private InputField _nickname;
        private InputField _password;

        private ScreenManager _screenManager;
        
        [Inject]
        public void InitializeDependencies(ScreenManager screenManager)
        {
            _screenManager = screenManager;
        }
        
        private void Start()
        {
            _login = transform.Find("RegisterForm/Login").GetComponent<InputField>();
            _nickname = transform.Find("RegisterForm/Nickname").GetComponent<InputField>();
            _password = transform.Find("RegisterForm/Password").GetComponent<InputField>();
        }

        private void OnEnable()
        {
            AuthService.Instance.OnRegister += OnRegister;
            AuthService.Instance.OnLogin += OnLogin;
        }

        public void Register()
        {
            AuthService.Instance.Register(
                _login.text,
                _nickname.text,
                _password.text
            );
        }

        private void OnDisable()
        {
            AuthService.Instance.OnRegister -= OnRegister;
            AuthService.Instance.OnLogin -= OnLogin;
        }
        
        public void OnRegister(RegistrationResponse response)
        {
            if (response.HasErrors)
            {
                //TODO
            }
           
        }
        
        public void OnLogin(AuthenticationResponse response)
        {
            if (!response.HasErrors)
            {
                ShowInstead(MainMenu);
            }
            else
            {
                //TODO
            }
        }
    }
}