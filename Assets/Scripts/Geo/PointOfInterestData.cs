using System;
using System.Collections.Generic;
using GameSparks.Core;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking;
using HauntedCity.Utils.Extensions;
using Mapbox.Utils;
using Zenject;

namespace HauntedCity.Geo
{
    [Serializable]
    public class PointOfInterestData
    {
        private AuthService _authService;

        public Vector2d LatLon { get; private set; }
        public Dictionary<string, int> Enemies { get; private set; }
        public string Poid { get; set; }
        public string Uoid { get;  set; }
        public string DisplayName { get;  set; }


        public POIMoney Money { get;   private set; }
        public POIShield Shield { get; private set; }

        public void SetGSData(GSData data)
        {
            var coords = data.GetGSData("geometry").GetFloatList("coordinates").ToArray();
            LatLon =new Vector2d(coords[0], coords[1]);
            var properties = data.GetGSData("properties");
            DisplayName = properties.GetString("owner_display_name");
            Uoid = properties.GetString("uoid");

            Poid = data.GetId();
            
            var enemies = properties.GetGSData("ghosts_num");
            Enemies.Clear();
            foreach (string ghostType in GameConfiguration.AllowableEnemies)
            {
                int? count = enemies.GetInt(ghostType);
                if (count != null)
                {
                    Enemies.Add(ghostType, (int)count);
                }
            }
            
            Money = new POIMoney(
                properties.GetInt("income_level" ) ??  1,
                properties.GetInt("current_money") ?? 0
            );
            
            Shield = new POIShield(
                properties.GetInt("shields_level" ) ??  1,
                properties.GetInt("current_shields") ?? 0
            );
                  
        }

        public bool IsYour()
        {
            return DisplayName == _authService.Nickname;//TODO Проверять по ID
        }

        public PointOfInterestData(AuthService authService)
        {
            _authService = authService;
            Enemies = new Dictionary<string, int>();
        }
    }
}