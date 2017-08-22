using System;
using System.Collections.Generic;
using Facebook.Unity;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using HauntedCity.Networking.Interfaces;
using UnityEngine;

namespace HauntedCity.Networking.Social
{
    public class FacebookAuth:ISocialAuth
    {

        private bool _isLogin;
        
        public event Action<AuthenticationResponse> OnAuthDone;
        public void DoAuth()
        {
            if (GS.Available)
            {
                if (!FB.IsInitialized)
                {
                    FB.Init(connectGStoFB);
                }
                else
                {
                    FB.ActivateApp();
                    connectGStoFB();
                }
            }
        }

        public void Logout()
        {
            _isLogin = false;
            FB.LogOut();
        }


        private void connectGStoFB()
        {
            _isLogin = false;
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
                var perms = new List<string>() {"public_profile", "email", "user_friends"};
                FB.LogInWithReadPermissions(perms, (fb_login_response) =>
                {
                    if (FB.IsLoggedIn)
                    {
                        new FacebookConnectRequest()
                            .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                            .SetDoNotLinkToCurrentPlayer(true)
                            .SetSwitchIfPossible(true)
                            .Send((gs_auth_response) =>
                            {
                                if(_isLogin) return;
                                _isLogin = true;
                                if (OnAuthDone != null)
                                {
                                    OnAuthDone(gs_auth_response);
                                }
                            });
                    }
                    else
                    {
                        Debug.LogWarning("FB login failed : " + fb_login_response.Error);
                    }
                });
            }
            else
            {
                Debug.LogWarning("FB initialization assumption failed");
                DoAuth();
            }
        }
    }
}