using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSparksBattle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public event EventHandler<POI_START_CAP_ev_arg> OnPOIStartCap;
    public class POI_START_CAP_ev_arg
    {
        public bool isStarted;
        public bool isError;
    }


    public void sendStartBattle(string poid)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("POI_START_CAP")
            .SetEventAttribute("POI_ID", poid)
            .Send((response) =>
            {
                Debug.Log(response.JSONString);
                POI_START_CAP_ev_arg arg = new POI_START_CAP_ev_arg();
                if (!response.HasErrors)
                {
                    SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                    string result = root["scriptData"]["started"];
                    arg.isStarted = (result == "OK");
                    arg.isError = false;           
                }
                else
                {
                    arg.isError = false;
                }
                OnPOIStartCap(this, arg);
            });
    }//start cap

}
