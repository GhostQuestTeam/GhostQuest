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

        public GameObject ErrorField;

        [Inject] private AuthService _authService;
        
        private void Start()
        {
            _login = transform.Find("RegisterForm/Login").GetComponent<InputField>();
            _nickname = transform.Find("RegisterForm/Nickname").GetComponent<InputField>();
            _password = transform.Find("RegisterForm/Password").GetComponent<InputField>();
        }

        private void OnEnable()
        {
            _authService.OnRegister += OnRegister;
        }

        public void Register()
        {
            _authService.Register(
                _login.text,
                _nickname.text,
                _password.text
            );
        }

        private void OnDisable()
        {
            _authService.OnRegister -= OnRegister;
        }
        
        public void OnRegister(RegistrationResponse response)
        {
            if (response.HasErrors)
            {
                ErrorField.SetActive(true);
            }
           
        }
        
        protected override void OnShow()
        {
            ErrorField.SetActive(false);
        }
        
        public void OnLogin(AuthenticationResponse response)
        {
            if (!response.HasErrors)
            {
                ShowInstead(MainMenu);
            }
            else
            {
                ErrorField.SetActive(true);    
            }
        }
    }
}