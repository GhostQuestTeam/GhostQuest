using GameSparks.Api.Responses;
using HauntedCity.Networking;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HauntedCity.UI
{
    public class RegistrationPanel : MonoBehaviour
    {
        public Animator MainMenu;
        
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

            AuthService.Instance.OnRegister += OnRegister;
        }

        public void Register()
        {
            AuthService.Instance.Register(
                _login.text,
                _nickname.text,
                _password.text
            );
        }

        private void OnDestroy()
        {
            AuthService.Instance.OnRegister -= OnRegister;
        }
        
        public void OnRegister(RegistrationResponse response)
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
    }
}