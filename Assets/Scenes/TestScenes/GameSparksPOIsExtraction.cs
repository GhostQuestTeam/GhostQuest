using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;

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

    public HashSet<Vector2d> _points = new HashSet<Vector2d>();

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
        string sEvArg = JsonUtility.ToJson(evArg);
        Debug.Log(sEvArg);

        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("GET_POIS")
            .SetEventAttribute("POS", sEvArg)
            .Send((response) =>
           {
               Debug.Log(response.JSONString);
               if (!response.HasErrors)
               {
                   SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                   foreach(SimpleJSON.JSONNode node in root["scriptData"]["points"].AsArray)
                   {
                       SimpleJSON.JSONArray coords = node["geometry"]["coordinates"].AsArray;
                       float lat = coords[0].AsFloat;
                       float lon = coords[1].AsFloat;
                       _points.Add(new Vector2d(lat, lon));
                       Debug.Log(lat.ToString() + " " + lon.ToString());
                   }
               }
               else
               {
                   retrievePoints(depth - 1);
               }
           });
    }
	
}
