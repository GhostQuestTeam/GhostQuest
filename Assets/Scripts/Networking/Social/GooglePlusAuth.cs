﻿using System;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GooglePlayGames;
using HauntedCity.Networking.Interfaces;

namespace HauntedCity.Networking.Social
{
    public class GooglePlusAuth:ISocialAuth
    {
        public event Action<AuthenticationResponse> OnAuthDone;
        public void DoAuth()
        {
            UnityEngine.Social.localUser.Authenticate((isSuccess) => {
                if(isSuccess)
                {
                    new GooglePlusConnectRequest()
                        .SetAccessToken(PlayGamesPlatform.Instance.GetIdToken())
                        .SetDoNotLinkToCurrentPlayer(true)
                        .SetSwitchIfPossible(false)
                        .Send(gp_login_response =>
                        {
                            if(OnAuthDone != null)
                            {
                                OnAuthDone(gp_login_response);
                            }
                        });
                }
            });
        }

        public void Logout()
        {
            PlayGamesPlatform.Instance.SignOut();
        }

        public GooglePlusAuth()
        {
            PlayGamesPlatform.Activate();
        }
    }
}