using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;

namespace HauntedCity.Geo
{
    public class LocationProviderWrapper:MonoBehaviour
    {
        private ILocationProvider _locationProvider;
        
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


        public Vector2d CurPos
        {
            get
            {
            
                #if UNITY_EDITOR
                    return LocationProvider.Location;
                #else
                    return new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
                #endif
            }
        }
    }
}