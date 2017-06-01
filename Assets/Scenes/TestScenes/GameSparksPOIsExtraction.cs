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

    public float fake_lat = 55.5f;
    public float fake_lon = 37.5f;

    // Use this for initialization
    void Start () {
        doFakeAuth("a1", "a1", 3);
        retrievePoints();
	}

    void doFakeAuth(string unm, string pass, int depth)
    {
        if (depth == 0)
            return;

        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(unm)
            .SetPassword(pass)
            .Send((responseAuth1) =>
            {
                if(responseAuth1.HasErrors)
                {
                    Debug.Log("Failed to auth on depth " + depth.ToString());

                    new GameSparks.Api.Requests.RegistrationRequest()
                    .SetDisplayName(unm)
                    .SetUserName(unm)
                    .SetPassword(pass)
                    .Send((responseReg) => 
                    {
                        if(!responseReg.HasErrors)
                        {
                            Debug.Log("Succeded to register on depth " + depth.ToString());
                            doFakeAuth(unm, pass, depth - 1);
                        }
                        else
                        {
                            Debug.Log("Failed to register on depth " + depth.ToString());
                        }
                    });
                }
            });
    }

    void retrievePoints()
    {

        GetPOIsEventArg evArg = new GetPOIsEventArg(fake_lat, fake_lon);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GET_POIS")
            .SetEventAttribute("POS", JsonUtility.ToJson(evArg))
            .Send((response) =>
           {
               if(!response.HasErrors)
               {
                   Debug.Log(response.JSONString);
               }
           });
    }
	
}
