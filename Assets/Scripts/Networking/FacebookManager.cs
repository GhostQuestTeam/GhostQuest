using System;
using System.Collections.Generic;
using Facebook.Unity;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;
using UnityEngine;

namespace HauntedCity.Networking
{
    public class FacebookManager : MonoBehaviour
    {
        public Action<AuthenticationResponse> OnAuthDone;

        public bool IsAuthenticated { get; private set; }


        // Use this for initialization
        void Start()
        {
            //OnAuthDone += testAuth;
            //doAuth();
        }

        private void testAuth(AuthenticationResponse response)
        {
            OnAuthDone -= testAuth;
            for (int i = 0; i < 10; ++i)
            {
                Debug.Log(this.ToString() + " " + response.JSONString);
            }
        }


        public void doAuth()
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


        private void connectGStoFB()
        {
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
                                if (OnAuthDone != null)
                                {
                                    IsAuthenticated = true;
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
                doAuth();
            }
        }
    }
}