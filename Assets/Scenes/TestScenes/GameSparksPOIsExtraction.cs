using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Core;
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

    public HashSet<ExtractedPointMetadata> _points = new HashSet<ExtractedPointMetadata>();

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
        public HashSet<ExtractedPointMetadata> points;

        public POIsExtractedEventArgs(HashSet<ExtractedPointMetadata> p)
        {
            points = p;
        }
    }

    public class ExtractedPointMetadata
    {
        public GSRequestData SparksData
        {
            get
            {
                var result = new GSRequestData();
                return result;
            }
        }
        
        public Vector2d LatLon;
        public Dictionary<string, int> enemies;
        public string uoid;
        public string poid;
    }

    public event EventHandler<POIsExtractedEventArgs> OnPOIsExtracted;

    // Use this for initialization
    void Start()
    {
        
        //StartCoroutine(performExtraction());
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

    public void UpdatePoint(ExtractedPointMetadata point)
    {
        throw new System.NotImplementedException();
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
                        string uoid = node["properties"]["uoid"];
                        string poid = node["_id"]["$oid"];

                        SimpleJSON.JSONNode enemies = node["properties"]["ghosts_num"];
                        Dictionary<string, int> enemiesDict = new Dictionary<string, int>();
                        enemiesDict.Add("shadow_skull", enemies["shadow_skull"]);
                        enemiesDict.Add("devil_mask", enemies["devil_mask"]);
                        enemiesDict.Add("skull_ghost", enemies["skull_ghost"]);

                        ExtractedPointMetadata pointMeta = new ExtractedPointMetadata();
                        pointMeta.LatLon = new Vector2d(lat, lon);
                        pointMeta.uoid = uoid;
                        pointMeta.poid = poid;
                        pointMeta.enemies = enemiesDict;

                        _points.Add(pointMeta);
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