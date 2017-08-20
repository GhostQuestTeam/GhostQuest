using System;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using HauntedCity.Networking.Interfaces;
using UnityEngine;
using Zenject;

namespace HauntedCity.Networking
{
    public class AuthService
    {
        public enum AuthType
        {
            Simple,
            Facebook,
            GooglePlus
        }

        private AuthType lastAuthType;
        private ISocialAuth _facebookAuth;
        private ISocialAuth _googleAuth;


        public AuthService(ISocialAuth facebookAuth, ISocialAuth googleAuth)
        {
            _facebookAuth = facebookAuth;
            _googleAuth = googleAuth;
            _facebookAuth.OnAuthDone += _OnSocialLogin;
            _googleAuth.OnAuthDone += _OnSocialLogin;
        }

        //Что-то по нормальному не получается сделать(
        public bool _isLogin;

        public AuthenticationResponse LastAuthResponse = null;

        public event Action<RegistrationResponse> OnRegister;
        public event Action<AuthenticationResponse> OnLogin;

        public string Nickname { get; private set; }
        public string UserId { get; private set; }



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
                        if (OnRegister != null) OnRegister(response);
                    }
                );
        }

        private void _OnSocialLogin(AuthenticationResponse response)
        {
            if (!response.HasErrors)
            {
                _isLogin = true;

                Nickname = response.DisplayName;
                UserId = response.UserId;
            }
            if (OnLogin != null) OnLogin(response);
        }

        public void Login(string login, string password)
        {
            lastAuthType = AuthType.Simple;
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
                        UserId = response.UserId;
                        Debug.Log("Player Authenticated...");
                    }
                    else
                    {
                        Debug.Log("Error Authenticating Player...");
                    }
                    if (OnLogin != null) OnLogin(response);
                });
        }

        public void SocialAuth(AuthType authType)
        {
            lastAuthType = authType;
            switch (authType)
            {
                case AuthType.Facebook:
                    _facebookAuth.DoAuth();
                    break;
                case AuthType.GooglePlus:
                    _googleAuth.DoAuth();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("authType", authType, null);
            }
        }

        public void Logout()
        {
            GS.GSPlatform.AuthToken = null;
            _isLogin = false;
            switch (lastAuthType)
            {
                case AuthType.Facebook:
                    _facebookAuth.Logout();       
                    break;
                case AuthType.GooglePlus:
                    _googleAuth.Logout();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}