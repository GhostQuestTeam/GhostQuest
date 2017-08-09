using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Core;
using HauntedCity.Geo;
using HauntedCity.Networking;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Location;
using Zenject;

[Serializable]
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

    public HashSet<PointOfInterestData> _points = new HashSet<PointOfInterestData>();

    [Inject] private AuthService _authService;
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
            #if UNITY_EDITOR
                return LocationProvider.Location;
            #else
                return new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
            #endif
            }
            else
            {
                return new Vector2d(fake_lat, fake_lon);
            }
        }
    }

    public class POIsExtractedEventArgs : EventArgs
    {
        public HashSet<PointOfInterestData> points;

        public POIsExtractedEventArgs(HashSet<PointOfInterestData> p)
        {
            points = p;
        }
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
        if (_authService.IsAuthenticated)
        {
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


//    public void UpdatePoint(ExtractedPointMetadata point)
//    {
//        new GameSparks.Api.Requests.LogEventRequest()
//            .SetEventKey("UPDATE_POINT")
//            .SetEventAttribute("POINT_ID", point.poid)
//            .SetEventAttribute("POINT_DATA", point.SparksData)
//            .Send((response) =>
//            {
//                if (!response.HasErrors)
//                {
//                    Debug.Log("Point updated...");
//                }
//                else
//                {
//                    Debug.Log("Error on save point..");
//                }
//            });
//    }


    void retrievePoints(int depth)
    {
        if (depth == 0)
            return;

        FindObjectOfType<DebugPanel>().Log(CurPos.ToString());
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
                    var pointsGSData = response.ScriptData.GetGSDataList("points");
                    foreach (GSData pointGsData in pointsGSData)
                    {
                       
                        var pointMeta = new PointOfInterestData();
                        pointMeta.SetGSData(pointGsData);

                        _points.Add(pointMeta);
                        //Debug.Log(lat.ToString() + " " + lon.ToString());
                    }
                    if (OnPOIsExtracted != null)
                        OnPOIsExtracted(this, new POIsExtractedEventArgs(_points));
                }
                else
                {
                    //retrievePoints(depth - 1);
                }
            });
    }
}