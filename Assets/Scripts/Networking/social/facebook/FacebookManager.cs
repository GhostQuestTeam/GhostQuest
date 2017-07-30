using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookManager : MonoBehaviour {

    public System.Action<GameSparks.Api.Responses.AuthenticationResponse> OnAuthDone;

	// Use this for initialization
	void Start () {
        //OnAuthDone += testAuth;
        //doAuth();
	}

    private void testAuth(GameSparks.Api.Responses.AuthenticationResponse response)
    {
        OnAuthDone -= testAuth;
        for(int i = 0; i < 10; ++i)
        {
            Debug.Log(this.ToString() + " " + response.JSONString);
        }
    }
    

    public void doAuth()
    {
        if(GameSparks.Core.GS.Available)
        {
            if(!FB.IsInitialized)
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
        if(FB.IsInitialized)
        {
            FB.ActivateApp();
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, (fb_login_response) =>
            {
                if(FB.IsLoggedIn)
                {
                    new GameSparks.Api.Requests.FacebookConnectRequest()
                        .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                        .SetDoNotLinkToCurrentPlayer(false)
                        .SetSwitchIfPossible(true)
                        .Send((gs_auth_response) => {
                            if(OnAuthDone != null)
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
            doAuth();
        }
    }


}
