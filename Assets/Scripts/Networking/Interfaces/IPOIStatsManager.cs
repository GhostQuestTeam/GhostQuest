using System;
using System.Collections.Generic;
using HauntedCity.Geo;
using Mapbox.Utils;

namespace HauntedCity.Networking.Interfaces
{
    public class POIsExtractedEventArgs : EventArgs
    {
        public HashSet<PointOfInterestData> points;

        public POIsExtractedEventArgs(HashSet<PointOfInterestData> p)
        {
            points = p;
        }
    }
    
    public interface IPOIStatsManager
    {
        event EventHandler<POIsExtractedEventArgs> OnPOIsExtracted;
        void TakeMoney(string pointId);
        void UpgradeShield(string pointId);
        void UpgradeIncome(string pointId);
        void RestoreShield(string pointId);
        void AttackShield(string pointId);
        void RetrievePoints(int depth, Vector2d pivot);
    }
}