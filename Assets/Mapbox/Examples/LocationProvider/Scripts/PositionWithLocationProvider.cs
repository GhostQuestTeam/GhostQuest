namespace Mapbox.Examples.LocationProvider
{
    using Mapbox.Unity.Location;
    using Mapbox.Unity.Utilities;
    using Mapbox.Unity.MeshGeneration;
    using UnityEngine;
	using Mapbox.Utils;

    public class PositionWithLocationProvider : MonoBehaviour
	{
        /// <summary>
        /// The rate at which the transform's position tries catch up to the provided location.
        /// </summary>
		[SerializeField]
		float _positionFollowFactor;

        /// <summary>
        /// Use a mock <see cref="T:Mapbox.Unity.Location.TransformLocationProvider"/>,
        /// rather than a <see cref="T:Mapbox.Unity.Location.EditorLocationProvider"/>. 
        /// </summary>
        [SerializeField]
        bool _useTransformLocationProvider;

		public Vector2d _myCurrentLocation;
		public GameObject _mapControllerObj;
		private MapController _mapController;

        /// <summary>
        /// The location provider.
        /// This is public so you change which concrete <see cref="T:Mapbox.Unity.Location.ILocationProvider"/> to use at runtime.
        /// </summary>
		ILocationProvider _locationProvider;
		public ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
                    _locationProvider = _useTransformLocationProvider ? 
                        LocationProviderFactory.Instance.TransformLocationProvider : LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
			set
			{
				if (_locationProvider != null)
				{
					_locationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;

				}
				_locationProvider = value;
				_locationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			}
		}

		Vector3 _targetPosition;

		void Start()
		{
			LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
			_mapController = _mapControllerObj.GetComponent<MapController> ();
		}

		void OnDestroy()
		{
			if (LocationProvider != null)
			{
				LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
			}
		}

		void LocationProvider_OnLocationUpdated(object sender, LocationUpdatedEventArgs e)
		{
            if (MapController.ReferenceTileRect == null)
            {
                return;
            }

			_myCurrentLocation = e.Location;
            _targetPosition = Conversions.GeoToWorldPosition(e.Location,
                                                             MapController.ReferenceTileRect.Center, 
                                                             MapController.WorldScaleFactor).ToVector3xz();
            Vector2d myCurAbsWorldPos = Conversions.GeoToWorldPosition(e.Location, new Vector2d(0, 0));
			Vector2 tms = Conversions.MetersToTile(myCurAbsWorldPos, _mapController.Zoom);
            //Debug.Log(tms);
            //_mapController.Request (tms, _mapController.Zoom);
            spawnNearbyTiles(tms);
		}

		void Update()
		{
			transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
		}

        void spawnNearbyTiles(Vector2 tms)
        {
            Vector4 range = _mapController.Range;
            for (int i = (int)(tms.x - range.x); i <= (tms.x + range.z); i++)
            {
                for (int j = (int)(tms.y - range.y); j <= (tms.y + range.w); j++)
                {
                    _mapController.Request(new Vector2(i, j), _mapController.Zoom);
                }
            }
        }//fn

	}
}
