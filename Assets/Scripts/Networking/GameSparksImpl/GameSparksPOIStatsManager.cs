using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameSparks.Core;
using HauntedCity.Geo;
using HauntedCity.Networking.Interfaces;
using HauntedCity.Utils.Extensions;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;
using Zenject;

namespace HauntedCity.Networking.GameSparksImpl
{
    public class GameSparksPOIStatsManager:IPOIStatsManager
    {
        public HashSet<PointOfInterestData> _points = new HashSet<PointOfInterestData>();
      
        
        [Inject] private AuthService _authService;

        public event EventHandler<POIsExtractedEventArgs> OnPOIsExtracted;

        
        
        private enum AllowableActions 
        {
            takeMoney,
            upgradeShield,
            upgradeIncome,
            restoreShield,
            attackShield,
            spawnGhost,
            applyBattleResult
        }
        
        private void _PerformAction(string pointId, AllowableActions action)
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("POI_ACTION")
                .SetEventAttribute("POI_ID", pointId)
                .SetEventAttribute("ACTION", action.ToString())
                .Send(null);//TODO
        }
        
        private void _PerformAction(string pointId, GSRequestData data ,AllowableActions action)
        {
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("POI_ACTION")
                .SetEventAttribute("POI_ID", pointId)
                .SetEventAttribute("PARAMS", data)
                .SetEventAttribute("ACTION", action.ToString())
                .Send(null);//TODO
        }
                
        public void TakeMoney(string pointId)
        {
            _PerformAction(pointId, AllowableActions.takeMoney);
        }

        public void UpgradeShield(string pointId)
        {
            _PerformAction(pointId, AllowableActions.upgradeShield);
        }

        public void UpgradeIncome(string pointId)
        {
            _PerformAction(pointId, AllowableActions.upgradeIncome);
        }

        public void RestoreShield(string pointId)
        {
            _PerformAction(pointId, AllowableActions.restoreShield);
        }

        public void AttackShield(string pointId)
        {
            _PerformAction(pointId, AllowableActions.attackShield);
        }

        public void SpawnGhost(string pointId, string ghostId)
        {
            _PerformAction(
                pointId,
                new GSRequestData().AddString("ghostID", ghostId),
                AllowableActions.spawnGhost
            );
        }

        public void ApplyBattleResult(string pointId, Dictionary<string, int> kills)
        {
            _PerformAction(
                pointId,
                new GSRequestData().AddObject("GHOSTS", kills.ToGsRequestData()),
                AllowableActions.applyBattleResult
            );
        }

        public void RetrievePoints(int depth, Vector2d pivot)
        {
            if (depth == 0)
                return;

            var gsAttr = new GSRequestData()
                .AddNumber("lat", (float) pivot.x)
                .AddNumber("lon", (float) pivot.y);
            
            new GameSparks.Api.Requests.LogEventRequest()
                .SetEventKey("GET_POIS")
                .SetEventAttribute("POS", gsAttr)
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
                       
                            var pointMeta = new PointOfInterestData(_authService);
                            pointMeta.SetGSData(pointGsData);

                            _points.Add(pointMeta);
                            //Debug.Log(lat.ToString() + " " + lon.ToString());
                        }
                        if (OnPOIsExtracted != null)
                            OnPOIsExtracted(this, new POIsExtractedEventArgs(_points));
                    }
                    else
                    {
                         RetrievePoints(depth - 1, pivot);
                    }
                });
        }
    }
}