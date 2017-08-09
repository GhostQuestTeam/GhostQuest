using System;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class AuthService
    {

        //Что-то по нормальному не получается сделать(
        public bool _isLogin;
        public AuthenticationResponse LastAuthResponse = null;

        public event Action<RegistrationResponse> OnRegister;
        public event Action<AuthenticationResponse> OnLogin;
        
        public string Nickname { get; private set; }


        public bool IsAuthenticated
        {
            get { return _isLogin; /*GS.GSPlatform.AuthToken != null;*/ }
        }
        
        public void Register(string login, string nickname, string password)
        {
            new GameSparks.Api.Requests.RegistrationRequest()
                .SetDisplayName(nickname)
                .SetPassword(password)
                .SetUserName(login)
                .Send((response) =>
                    {
                        if (!response.HasErrors)
                        {
                            Debug.Log("Player Registered");
                            Login(login, password);
                        }
                        else
                        {
                            Debug.Log("Error Registering Player");
                        }
                        if (OnRegister != null)
                        {
                            OnRegister(response);
                        }
                    }
                );
        }

        public void Login(string login, string password)
        {
            new GameSparks.Api.Requests.AuthenticationRequest()
                .SetUserName(login)
                .SetPassword(password)
                .Send((response) =>
                {
                    LastAuthResponse = response;
                    if (!response.HasErrors)
                    {
                        _isLogin = true;

                        Nickname = response.DisplayName;
                        Debug.Log("Player Authenticated...");
                    }
                    else
                    {
                        Debug.Log("Error Authenticating Player...");
                    }
                    if (OnLogin != null)
                    {
                        OnLogin(response);
                    }
                });
        }

        public void Logout()
        {
            GS.GSPlatform.AuthToken = null;
            _isLogin = false;
        }
    }
}