using System;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GooglePlayGames;
using HauntedCity.Networking.Interfaces;

namespace HauntedCity.Networking.Social
{
    public class GooglePlusAuth : ISocialAuth
    {
        private bool _isLogin;

        public event Action<AuthenticationResponse> OnAuthDone;

        public void DoAuth()
        {
//            UnityEngine.Social.localUser.Authenticate((isSuccess) =>
//            {
//                if (isSuccess)
//                {
//                    var code = PlayGamesPlatform.Instance.GetServerAuthCode();
//                    var displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
////                    var email = PlayGamesPlatform.Instance.GetUserEmail();
//                    new GooglePlayConnectRequest()
//                        .SetCode(code)
//                        //.SetAccessToken (PlayGamesPlatform.Instance.GetAccessToken ())
//                        .SetDoNotLinkToCurrentPlayer(true)
//                        .SetSwitchIfPossible(true)
////                        .SetRedirectUri("http://www.gamesparks.com/oauth2callback")
//                        .SetDisplayName(displayName)
//                        .Send((googleplayAuthResponse) =>
//                        {
//                            if (_isLogin) return;
//                            _isLogin = true;
//                            if (OnAuthDone != null)
//                            {
//                                OnAuthDone(googleplayAuthResponse);
//                            }
//                        });
//                }
//
////                new GooglePlayConnectRequest()
////                    .SetAccessToken(PlayGamesPlatform.Instance.GetIdToken())
////                    .SetDoNotLinkToCurrentPlayer(true)
////                    .SetSwitchIfPossible(true)
////                    .Send(gp_login_response =>
////                    {
////                        if (_isLogin) return;
////                        _isLogin = true;
////                        if (OnAuthDone != null)
////                        {
////                            OnAuthDone(gp_login_response);
////                        }
////                    });
//            });
        }


        public void Logout()
        {
            _isLogin = false;
            PlayGamesPlatform.Instance.SignOut();
        }

        public GooglePlusAuth()
        {
            PlayGamesPlatform.Activate();
        }
    }
}