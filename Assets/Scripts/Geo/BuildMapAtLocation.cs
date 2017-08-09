using UnityEngine;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.Location;

namespace HauntedCity.Geo
{
    /// <summary>
    /// Override the map center (latitude, longitude) for a MapController, based on the DefaultLocationProvider.
    /// This will enable you to generate a map for your current location, for example.
    /// </summary>
    public class BuildMapAtLocation : MonoBehaviour
    {
        [SerializeField] MapController _mapController;

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
        }

        void Start()
        {
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
        }

        void LocationProvider_OnLocationUpdated(object sender, LocationUpdatedEventArgs e)
        {
            LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            _mapController.LatLng = string.Format("{0}, {1}", e.Location.x, e.Location.y);
            _mapController.enabled = true;
        }
    }
}