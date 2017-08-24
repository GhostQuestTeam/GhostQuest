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

            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);

            PlayGamesPlatform.Activate();
            log.text = PlayGamesPlatform.Instance.GetUserDisplayName();
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
                    log.text = "Success UnityEngine.Social.localUser.Authenticate";
                    //We set the access token to the newly added built in funtion in Google Play Games to get our token
//                    new GooglePlayConnectRequest().SetAccessToken(PlayGamesPlatform.Instance.GetIdToken())
//                        .SetDoNotLinkToCurrentPlayer(true)
//                        .Send((googleAuthResponse) =>
//                        {
//                            log.text = googleAuthResponse.JSONString;
//                            if (!googleAuthResponse.HasErrors)
//                            {
//                                Debug.Log(googleAuthResponse.DisplayName + " Logged In!");
//                                log.text = googleAuthResponse.DisplayName + " Logged In!";
//                            }
//                            else
//                            {
//                                Debug.Log("Not Logged In!");
//                                log.text = googleAuthResponse.JSONString;
//                            }
//                        });
                    var code = PlayGamesPlatform.Instance.GetServerAuthCode();
                    string displayName = PlayGamesPlatform.Instance.GetUserDisplayName();
                    string email = PlayGamesPlatform.Instance.GetUserEmail();
                    new GooglePlayConnectRequest()
                        .SetCode(code)
                        //.SetAccessToken (PlayGamesPlatform.Instance.GetAccessToken ())
                        .SetDoNotLinkToCurrentPlayer(true)
                        .SetSwitchIfPossible(true)
//                        .SetRedirectUri("http://www.gamesparks.com/oauth2callback")
                        .SetDisplayName(displayName)
                        .Send((googleplayAuthResponse) =>
                        {
                            if (!googleplayAuthResponse.HasErrors)
                            {
                                Debug.Log(googleplayAuthResponse.DisplayName + " Logged In !");
                                log.text = "SUCCESSSSS    EMAIL : " + email + " ====== Status : ok"  +
                                           "  ==== Code : " + code + " ====== " + "AccessToken : " +
                                           PlayGamesPlatform.Instance.GetIdToken() + " ==== " +
                                           googleplayAuthResponse.DisplayName + "   :   " +
                                           googleplayAuthResponse.UserId + "   :   " + "Logged In! \n " +
                                           googleplayAuthResponse.JSONString;
                            }
                            else
                            {
                                Debug.Log("Failed To Login");
                                log.text = "ERRRRORRRR    EMAIL : " + email + " ====== Status : fail"  +
                                           " ==== Code : " + code + " ====== " + "AccessToken : " +
                                           PlayGamesPlatform.Instance.GetIdToken() + " ==== " +
                                           googleplayAuthResponse.JSONString;
                            }
                        });
                }
                else
                {
                    log.text = "Error UnityEngine.Social.localUser.Authenticate\n" + errors;
                }
            });
        }


        //Called from a UI Button
        public void GooglePlusAuthentication()
        {
        }
    }
}