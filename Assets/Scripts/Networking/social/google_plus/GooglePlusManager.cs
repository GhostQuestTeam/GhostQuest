using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GooglePlayGames;

public class GooglePlusManager : MonoBehaviour {

    public System.Action<AuthenticationResponse> OnAuthDone;

	void Start () {
        PlayGamesPlatform.Activate();
	}

    
    public void doAuth()
    {
        Social.localUser.Authenticate((isSuccess) => {
            if(isSuccess)
            {
                new GooglePlusConnectRequest()
                    .SetAccessToken(PlayGamesPlatform.Instance.GetIdToken())
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
    
}
