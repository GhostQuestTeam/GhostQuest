using UnityEngine;
using UnityEngine.UI;
using Networking;

namespace UI
{
    public class LoginPanel:MonoBehaviour
    {
        private InputField _login;
        private InputField _password;

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

        public void Logout()
        {
            AuthService.Instance.Logout();
        }
    }
}