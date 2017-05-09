using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity;
using Mapbox.Unity.Location;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;

public class PointOfInterestWithLocationProvider : MonoBehaviour {

	[SerializeField]
	float _positionFollowFactor;

	[SerializeField]
	bool _useTransformLocationProvider;

	private static Vector2d _myMapLocation = new Vector2d (55.8257f, 49.0538f);
	public GameObject _playerObject;
	public GameObject _btnToEnable;
	public float _myDistanceCutOff = 0.0005f;
	public float _debug_DistanceToPlayer;

	ILocationProvider _locationProvider;
	public ILocationProvider LocationProvider
	{
		private get
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
		_playerObject = GameObject.FindGameObjectWithTag ("Player");
		_btnToEnable = GameObject.FindGameObjectWithTag ("BtnToEnable");
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

		_targetPosition = Conversions.GeoToWorldPosition(_myMapLocation,
			MapController.ReferenceTileRect.Center, 
			MapController.WorldScaleFactor).ToVector3xz();
		
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
		checkDistanceToPlayer ();
	}

	void checkDistanceToPlayer() {
		Vector2d interestLocation = _myMapLocation;
		Vector2d playerLocation = _playerObject.GetComponent<Mapbox.Examples.LocationProvider.PositionWithLocationProvider> ()._myCurrentLocation;
		float distanceToInterest = (float)Vector2d.Distance (interestLocation, playerLocation);
		_debug_DistanceToPlayer = distanceToInterest;
		if (distanceToInterest < _myDistanceCutOff) {
			_btnToEnable.SetActive (true);
		} else {
			_btnToEnable.SetActive (false);
		}
	}//fn

}
	
