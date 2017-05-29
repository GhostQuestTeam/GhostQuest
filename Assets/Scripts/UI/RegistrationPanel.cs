using HauntedCity.Networking;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.UI
{
    public class RegistrationPanel : MonoBehaviour
    {
        private InputField _login;
        private InputField _nickname;
        private InputField _password;

        private void Start()
        {
            _login = transform.Find("RegisterForm/Login").GetComponent<InputField>();
            _nickname = transform.Find("RegisterForm/Nickname").GetComponent<InputField>();
            _password = transform.Find("RegisterForm/Password").GetComponent<InputField>();
        }

        public void Register()
        {
            AuthService.Instance.Register(
                _login.text,
                _nickname.text,
                _password.text
            );
        }
    }
}