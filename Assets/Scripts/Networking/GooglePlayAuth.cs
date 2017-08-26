using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Requests;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


namespace HauntedCity.Networking
{
    public class GooglePlayAuth : MonoBehaviour
    {
        public Text log;

        public void Start()
        {
            //Initialize Google Play

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
                .RequestIdToken()
                .RequestServerAuthCode(false).Build();
            PlayGamesPlatform.InitializeInstance(config);

            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

        }

        //Called from a UI Button
        public void GooglePlusStart()
        {
            log.text = PlayGamesPlatform.Instance.IsAuthenticated().ToString();
            //Start the Google Plus Overlay Login
            UnityEngine.Social.localUser.Authenticate((success, errors) =>
            {
                if (success)
                {
                    StartCoroutine(_GamesparksGoogleConnect());
//                    var code = PlayGamesPlatform.Instance.GetServerAuthCode();
//                    string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
//                    string email = PlayGamesPlatform.Instance.GetUserEmail();
//                    new GooglePlayConnectRequest()
//                        .SetCode(code)
//                        //.SetAccessToken (PlayGamesPlatform.Instance.GetAccessToken ())
//                        .SetDoNotLinkToCurrentPlayer(true)
//                        .SetSwitchIfPossible(true)
////                        .SetRedirectUri("http://www.gamesparks.com/oauth2callback")
//                        .SetDisplayName(displayName)
//                        .Send((googleplayAuthResponse) =>
//                        {
//                            if (!googleplayAuthResponse.HasErrors)
//                            {
//                                Debug.Log(googleplayAuthResponse.DisplayName + " Logged In !");
//                                log.text = "SUCCESSSSS    EMAIL : " + email + " ====== Status : ok"  +
//                                           "  ==== Code : " + code + " ====== " + "AccessToken : " +
//                                           PlayGamesPlatform.Instance.GetIdToken() + " ==== " +
//                                           googleplayAuthResponse.DisplayName + "   :   " +
//                                           googleplayAuthResponse.UserId + "   :   " + "Logged In! \n " +
//                                           googleplayAuthResponse.JSONString;
//                            }
//                            else
//                            {
//                                Debug.Log("Failed To Login");
//                                log.text = "ERRRRORRRR    EMAIL : " + email + " ====== Status : fail"  +
//                                           " ==== Code : " + code + " ====== " + "AccessToken : " +
//                                           PlayGamesPlatform.Instance.GetIdToken() + " ==== " +
//                                           googleplayAuthResponse.JSONString;
//                            }
//                        });
                }
                else
                {
                    log.text = "Error UnityEngine.Social.localUser.Authenticate\n" + errors;
                }
            });
        }

        private IEnumerator _GamesparksGoogleConnect()
        {
            var waitTime = 0.1f;
            var totalTime = 0f;
            var serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
            var idToken = PlayGamesPlatform.Instance.GetIdToken();

            while (serverAuthCode == null && idToken == null)
            {
                totalTime += waitTime;
                yield return new WaitForSecondsRealtime(waitTime);
                if (totalTime > 5f)
                {
                    break;
                }
                serverAuthCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                idToken = PlayGamesPlatform.Instance.GetIdToken();

            }

            var displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
            var email = PlayGamesPlatform.Instance.GetUserEmail();
            
            log.text += "Access token: " + (serverAuthCode ?? "null") + "\n";
            log.text += "Id token: " + (idToken ?? "null") + "\n";
            log.text += "DisplayName: " + (displayName ?? "null") + "\n";
            log.text += "Email: " + (email ?? "null") + "\n";



            new GooglePlayConnectRequest().SetAccessToken(idToken)
                .SetDisplayName(displayName)
                .SetDoNotLinkToCurrentPlayer(true)
//                .SetRedirectUri("http://www.gamesparks.com/oauth2callback")
                .Send((googleAuthResponse) =>
                {
                    log.text += googleAuthResponse.JSONString;
                    if (!googleAuthResponse.HasErrors)
                    {
                    }
                    else
                    {
                    }
                });
        }


        //Called from a UI Button
        public void GooglePlusAuthentication()
        {
        }
    }
}