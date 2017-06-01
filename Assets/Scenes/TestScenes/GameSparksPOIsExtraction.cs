using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GetPOIsEventArg
{
    public float lat;
    public float lon;
    public GetPOIsEventArg(float Lat, float Lon)
    {
        lat = Lat;
        lon = Lon;
    }
}

public class GameSparksPOIsExtraction : MonoBehaviour {

    public float fake_lat = 55.66f;
    public float fake_lon = 37.63f;

    // Use this for initialization
    void Start () {
        Debug.Log("Started!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        int depth = 5;
        doFakeAuth("a1", "a1", depth, depth);
	}

    void Update() {
        GameSparks.Core.GS.GSPlatform.RequestTimeoutSeconds = 100;
    }

    void doFakeAuth(string unm, string pass, int depthAuth, int depthRetrieve)
    {
        if (depthAuth == 0)
            return;

        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(unm)
            .SetPassword(pass)
            .Send((responseAuth1) =>
            {
                if(responseAuth1.HasErrors)
                {
                    Debug.Log("Failed to auth on depth " + depthAuth.ToString());

                    new GameSparks.Api.Requests.RegistrationRequest()
                    .SetDisplayName(unm)
                    .SetUserName(unm)
                    .SetPassword(pass)
                    .Send((responseReg) => 
                    {
                        if(!responseReg.HasErrors)
                        {
                            Debug.Log("Succeded to register on depth " + depthAuth.ToString());
                            doFakeAuth(unm, pass, depthAuth - 1, depthRetrieve);
                        }
                        else
                        {
                            Debug.Log("Failed to register on depth " + depthAuth.ToString());
                        }
                    });
                }
                else
                {
                    retrievePoints(depthRetrieve);
                }
            });
    }

    void retrievePoints(int depth)
    {
        if (depth == 0)
            return;

        GetPOIsEventArg evArg = new GetPOIsEventArg(fake_lat, fake_lon);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GET_POIS")
            .SetEventAttribute("POS", JsonUtility.ToJson(evArg))
            .Send((response) =>
           {
               if(!response.HasErrors)
               {
                   Debug.Log(response.JSONString);
                   SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                   root.ToString();
               }
               else
               {
                   retrievePoints(depth - 1);
               }
           });
    }
	
}
