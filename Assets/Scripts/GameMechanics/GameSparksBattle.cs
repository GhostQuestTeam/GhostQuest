using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HauntedCity.Networking;
using GameSparks.Api.Messages;

public class GameSparksBattle : MonoBehaviour {

	
	void Awake () {
        GameSparks.Api.Messages.ScriptMessage.Listener += ScriptMessageListener;
	}


    public event EventHandler<SCRIPT_MESSAGE_ev_arg> OnScriptMessage;
    public class SCRIPT_MESSAGE_ev_arg : EventArgs
    {
        public ScriptMessage message;
    }

    public event EventHandler<SCRIPT_MESSAGE_POI_OWNER_CHANGE_ev_arg> OnScriptMessagePOIOwnerChange;
    public class SCRIPT_MESSAGE_POI_OWNER_CHANGE_ev_arg : EventArgs
    {
        public ScriptMessage message;
        public bool isError;
        public bool hasPrevOwner;
        public string poid;
        public string newOwnerUoid;
        public string prevOwnerUoid;
        public string newOwnerDisplayName;
        public string prevOwnerDisplayName;
        public string newOwnerUserName;
        public string prevOwnerUserName;
    }

    void ScriptMessageListener(ScriptMessage message)
    {
        SCRIPT_MESSAGE_ev_arg arg = new SCRIPT_MESSAGE_ev_arg();
        arg.message = message;

        if(OnScriptMessage != null)
            OnScriptMessage(this, arg);

        string type = message.Data.GetString("type");

        if (type == null)
            return;

        if(type == "TYPE_POI_OWNER_CHANGE")
        {
            SCRIPT_MESSAGE_POI_OWNER_CHANGE_ev_arg ownerChangeArg = new SCRIPT_MESSAGE_POI_OWNER_CHANGE_ev_arg();
            ownerChangeArg.message = message;
            if(!message.HasErrors)
            {
                ownerChangeArg.isError = false;
                ownerChangeArg.poid = message.Data.GetString("poid");
                ownerChangeArg.newOwnerUoid = message.Data.GetString("newOwnerUoid");
                ownerChangeArg.newOwnerDisplayName = message.Data.GetString("newOwnerDisplayName");
                ownerChangeArg.newOwnerUserName = message.Data.GetString("newOwnerUserName");
                ownerChangeArg.hasPrevOwner = (bool)message.Data.GetBoolean("hasPrevOwner");
                if (ownerChangeArg.hasPrevOwner)
                {
                    ownerChangeArg.prevOwnerUoid = message.Data.GetString("prevOwnerUoid");
                    ownerChangeArg.prevOwnerDisplayName = message.Data.GetString("prevOwnerDisplayName");
                    ownerChangeArg.prevOwnerUserName = message.Data.GetString("prevOwnerUserName");
                }
            }
            else
            {
                ownerChangeArg.isError = true;
            }
            if(OnScriptMessagePOIOwnerChange != null)
                OnScriptMessagePOIOwnerChange(this, ownerChangeArg);
        }//type owner change


        if(type == "TYPE_POI_FAIL_CAPTURE")
        {
            //
            //TODO
            //
        }

    }//func

    public event EventHandler<POI_START_CAP_ev_arg> OnPOIStartCap;
    public class POI_START_CAP_ev_arg : EventArgs
    {
        public string poid;
        public bool isStarted;
        public bool isError;
    }


    public void sendStartCapture(string poid)
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
                    arg.poid = poid;           
                }
                else
                {
                    arg.isError = true;
                    arg.poid = poid;
                    arg.isStarted = false;
                }
                if(OnPOIStartCap != null)
                    OnPOIStartCap(this, arg);
            });
    }//start cap




    public event EventHandler<POI_SUCESS_CAP_ev_arg> OnPOISuccessCap;
    public class POI_SUCESS_CAP_ev_arg : EventArgs
    {
        public string poid;
        public bool isSuccess;
        public bool isError;
    }

    public void sendSuccessCapture(string poid)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("POI_SUCCESS_CAP")
            .SetEventAttribute("POI_ID", poid) //может этот параметр и не нужен - и так до этого понятно, что захватывалось
            .Send((response) =>
            {
                Debug.Log(response.JSONString);
                POI_SUCESS_CAP_ev_arg arg = new POI_SUCESS_CAP_ev_arg();
                if (!response.HasErrors)
                {
                    SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                    string result = root["scriptData"]["result"];
                    arg.isSuccess = (result == "OK");
                    arg.isError = false;
                    arg.poid = poid;
                }
                else
                {
                    arg.isError = true;
                    arg.poid = poid;
                    arg.isSuccess = false;
                }
                if(OnPOISuccessCap != null)
                    OnPOISuccessCap(this, arg);
            });
    }//sucess cap



    public event EventHandler<POI_FAIL_CAP_CONFIRM_ev_arg> OnPOIFailCapConfirm;
    public class POI_FAIL_CAP_CONFIRM_ev_arg : EventArgs
    {
        public string poid;
        public bool isSuccess;
        public bool isError;
    }

    public void sendFailCaptureConfirm(string poid)
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("POI_FAIL_CAP_CONFIRM")
            .SetEventAttribute("POI_ID", poid) //может этот параметр и не нужен - и так до этого понятно, что захватывалось
            .Send((response) =>
            {
                Debug.Log(response.JSONString);
                POI_FAIL_CAP_CONFIRM_ev_arg arg = new POI_FAIL_CAP_CONFIRM_ev_arg();
                if (!response.HasErrors)
                {
                    SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                    string result = root["scriptData"]["result"];
                    arg.isSuccess = (result == "OK");
                    arg.isError = false;
                    arg.poid = poid;
                }
                else
                {
                    arg.isError = true;
                    arg.poid = poid;
                    arg.isSuccess = false;
                }
                if (OnPOIFailCapConfirm != null)
                    OnPOIFailCapConfirm(this, arg);
            });
    }//fail cap confirm



    public event EventHandler<GET_LEADERBOARD_ev_arg> OnGetLeaderboard;
    public class GET_LEADERBOARD_ev_arg : EventArgs
    {
        public string rank;
        public string players;
        public bool isError;
    }

    public void sendGetLeaderboard()
    {
        new GameSparks.Api.Requests.LogEventRequest()
            .SetEventKey("POI_SUCCESS_CAP")
            .Send((response) =>
            {
                Debug.Log(response.JSONString);
                GET_LEADERBOARD_ev_arg arg = new GET_LEADERBOARD_ev_arg();
                if (!response.HasErrors)
                {
                    SimpleJSON.JSONNode root = SimpleJSON.JSON.Parse(response.JSONString);
                    string players = root["scriptData"]["players"];
                    string rank = root["scriptData"]["rank"];
                    arg.players = players;
                    arg.rank = rank;
                    arg.isError = false;
                }
                else
                {
                    arg.isError = true;
                    arg.players = null;
                    arg.rank = null;
                }
                if(OnGetLeaderboard != null)
                    OnGetLeaderboard(this, arg);
            });
    }//get leaderboard



}//main class
