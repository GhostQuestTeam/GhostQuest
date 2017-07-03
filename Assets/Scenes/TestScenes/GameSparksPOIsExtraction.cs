using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Core;
using HauntedCity.GameMechanics.Main;
using HauntedCity.GameMechanics.SkillSystem;
using HauntedCity.Geo;
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
        public HashSet<ExtractedPointMetadata> points;

        public POIsExtractedEventArgs(HashSet<ExtractedPointMetadata> p)
        {
            points = p;
        }
    }


    [Serializable]
    public class ExtractedPointMetadata
    {
        private PlayerGameStats _playerStats = GameController.GameStats;
        public const int RESTORE_SHIELD_PRICE = 3000;
        public const int SHIELD_UPGRADE_PRICE = 4000;
        public const int INCOME_UPGRADE_PRICE = 4000;

        public GSRequestData SparksData
        {
            get
            {
                var result = new GSRequestData();
                result.AddNumber("income_level", incomeLevel);
                result.AddNumber("shields_level", shieldLevel);
                result.AddNumber("current_money", currentMoney);
                result.AddNumber("current_shields", currentShield);
                return result;
            }
        }


        public Vector2d LatLon;
        public Dictionary<string, int> enemies;
        public int incomeLevel;
        public int shieldLevel;
        public int currentShield;
        public int currentMoney;

        public int MaxMoney
        {
            get { return incomeLevel * 900; }
        }

        public int MaxShield
        {
            get { return shieldLevel * 300; }
        }

        #region ActionsWithPoint //TODO Избавиться от копипасты

        public bool TryUpgradeShield()
        {
            if (_playerStats.Money >= SHIELD_UPGRADE_PRICE)
            {
                _playerStats.Money -= SHIELD_UPGRADE_PRICE;
                shieldLevel++;
                return true;
            }
            return false;
        }

        public bool TryUpgradeIncome()
        {
            if (_playerStats.Money >= INCOME_UPGRADE_PRICE)
            {
                _playerStats.Money -= INCOME_UPGRADE_PRICE;
                incomeLevel++;
                return true;
            }
            return false;
        }

        public bool TryRestoreShield()
        {
            if (_playerStats.Money >= RESTORE_SHIELD_PRICE)
            {
                _playerStats.Money -= RESTORE_SHIELD_PRICE;
                currentShield = MaxShield;
                return true;
            }
            return false;
        }

        public void GetMoney()
        {
            _playerStats.Money += currentMoney;
            currentMoney = 0;
        }

        #endregion


        public string displayName;
        public string uoid;
        public string poid;
        public string name;
        public string owner_display_name;
        public string owner_user_name;
        public int income_level;
        public int guards_level;
        public int shields_level;
        public int current_money;
        public int current_shields;
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
        if (AuthService.Instance.IsAuthenticated)
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
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("UPDATE_POINT")
            .SetEventAttribute("POINT_ID", point.poid)
            .SetEventAttribute("POINT_DATA", point.SparksData)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Point updated...");
                }
                else
                {
                    Debug.Log("Error on save point..");
                }
            });
    }


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
                    foreach (SimpleJSON.JSONNode node in root["scriptData"]["points"].AsArray)
                    {
                        SimpleJSON.JSONArray coords = node["geometry"]["coordinates"].AsArray;
                        float lat = coords[0].AsFloat;
                        float lon = coords[1].AsFloat;
                        string uoid = node["properties"]["uoid"];
                        string displayName = node["properties"]["owner_display_name"];
                        string poid = node["_id"]["$oid"];
                        string name = node["properties"]["name"];
                        string owner_display_name = node["properties"]["owner_display_name"];
                        string owner_user_name = node["properties"]["owner_user_name"];
                        int income_level = node["properties"]["income_level"].AsInt;
                        int guards_level = node["properties"]["guards_level"].AsInt;
                        int shields_level = node["properties"]["shields_level"].AsInt;
                        int current_money = node["properties"]["current_money"].AsInt;
                        int current_shields = node["properties"]["current_shields"].AsInt;

                        int currentMoney = node["properties"]["current_money"].AsInt;
                        int currenShields = node["properties"]["current_shields"].AsInt;
                        int incomeLevel = node["properties"]["income_level"].AsInt;
                        int shieldLevel = node["properties"]["shields_level"].AsInt;


                        SimpleJSON.JSONNode enemies = node["properties"]["ghosts_num"];
                        Dictionary<string, int> enemiesDict = new Dictionary<string, int>();

                        enemiesDict.Add("shadow_skull", enemies["shadow_skull"]);
                        enemiesDict.Add("devil_mask", enemies["devil_mask"]);
                        enemiesDict.Add("skull_ghost", enemies["skull_ghost"]);
                        enemiesDict.Add("white_skull", enemies["white_skull"]);
                        enemiesDict.Add("headless", enemies["headless"]);
                        enemiesDict.Add("skeleton", enemies["skeleton"]);

                        
                        ExtractedPointMetadata pointMeta = new ExtractedPointMetadata();
                        pointMeta.LatLon = new Vector2d(lat, lon);
                        pointMeta.uoid = uoid;
                        pointMeta.poid = poid;
                        pointMeta.name = name;
                        pointMeta.owner_display_name = owner_display_name;
                        pointMeta.owner_user_name = owner_user_name;
                        pointMeta.income_level = income_level;
                        pointMeta.guards_level = guards_level;
                        pointMeta.shields_level = shields_level;
                        pointMeta.current_money = current_money;
                        pointMeta.current_shields = current_shields;
                        pointMeta.enemies = enemiesDict;
                        pointMeta.displayName = displayName;

                        pointMeta.currentMoney = currentMoney;
                        pointMeta.currentShield = currenShields;
                        pointMeta.incomeLevel = incomeLevel;
                        pointMeta.shieldLevel = shieldLevel;

                        _points.Add(pointMeta);
                        Debug.Log(lat.ToString() + " " + lon.ToString());
                    }
                    OnPOIsExtracted(this, new POIsExtractedEventArgs(_points));
                }
                else
                {
                    //retrievePoints(depth - 1);
                }
            });
    }
}