using System;
using System.Collections;
using System.Collections.Generic;
using HauntedCity.Networking;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Location;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.Utilities;

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

public class GameSparksPOIsExtraction : MonoBehaviour
{
    public float fake_lat = 55.66f;
    public float fake_lon = 37.63f;

    public HashSet<Vector2d> _points = new HashSet<Vector2d>();

    ILocationProvider _locationProvider;

    public ILocationProvider LocationProvider
    {
        get
        {
            if (_locationProvider == null)
            {
                _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
            }
            return _locationProvider;
        }
        set { _locationProvider = value; }
    }

    public bool UseLocationProvider = true;

    public Vector2d CurPos
    {
        get
        {
            if (UseLocationProvider)
            {
                return LocationProvider.Location;
            }
            else
            {
                return new Vector2d(fake_lat, fake_lon);
            }
        }
    }

    public class POIsExtractedEventArgs : EventArgs
    {
        public HashSet<Vector2d> points;

        public POIsExtractedEventArgs(HashSet<Vector2d> p)
        {
            points = p;
        }
    }

    public event EventHandler<POIsExtractedEventArgs> OnPOIsExtracted;

    // Use this for initialization
    void Start()
    {
        
        StartCoroutine(performExtraction());
    }

    public int depth = 5;

    public void UpdatePointsNow()
    {
        if(AuthService.Instance.IsAuthenticated){
            retrievePoints(depth);
        }
        //doExtractionViaFakeAuth("a1", "a1", depth, depth);
    }

    IEnumerator performExtraction()
    {
        while (true)
        {
            UpdatePointsNow();
            yield return new WaitForSecondsRealtime(45);
        }
    }


    void doExtractionViaFakeAuth(string unm, string pass, int depthAuth, int depthRetrieve)
    {
        if (depthAuth == 0)
            return;

        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(unm)
            .SetPassword(pass)
            .Send((responseAuth1) =>
            {
                if (responseAuth1.HasErrors)
                {
                    Debug.Log("Failed to auth on depth " + depthAuth.ToString());

                    new GameSparks.Api.Requests.RegistrationRequest()
                        .SetDisplayName(unm)
                        .SetUserName(unm)
                        .SetPassword(pass)
                        .Send((responseReg) =>
                        {
                            if (!responseReg.HasErrors)
                            {
                                Debug.Log("Succeded to register on depth " + depthAuth.ToString());
                                doExtractionViaFakeAuth(unm, pass, depthAuth - 1, depthRetrieve);
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

        GetPOIsEventArg evArg = new GetPOIsEventArg((float) CurPos.x, (float) CurPos.y);
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
                    _points.Clear();
                    SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                    foreach (SimpleJSON.JSONNode node in root["scriptData"]["points"].AsArray)
                    {
                        SimpleJSON.JSONArray coords = node["geometry"]["coordinates"].AsArray;
                        float lat = coords[0].AsFloat;
                        float lon = coords[1].AsFloat;
                        _points.Add(new Vector2d(lat, lon));
                        Debug.Log(lat.ToString() + " " + lon.ToString());
                    }
                    OnPOIsExtracted(this, new POIsExtractedEventArgs(_points));
                }
                else
                {
                    retrievePoints(depth - 1);
                }
            });
    }
}