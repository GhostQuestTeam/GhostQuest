using System;
using GameSparks.Core;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking;
using HauntedCity.UI;
using HauntedCity.UI.PointInfo;
using HauntedCity.Utils.Extensions;
using Mapbox.Unity.Location;
using Mapbox.Unity.MeshGeneration;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using Zenject;

namespace HauntedCity.Geo
{
    public class PointOfInterestWithLocationProvider : MonoBehaviour
    {
        private MessageRetranslator _messageRetranslator;

        public MessageRetranslator messageRetranslator
        {
            get { return _messageRetranslator; }
            set
            {
                if (_messageRetranslator != null)
                {
                    _messageRetranslator.Unsubscribe(MessageType.POI_UPDATE, OnPOIUpdate);
                }
                _messageRetranslator = value;
                _messageRetranslator.Subscribe(MessageType.POI_UPDATE, OnPOIUpdate);
            }
        }


        [SerializeField] float _positionFollowFactor;

        [SerializeField] bool _useTransformLocationProvider;

        public Vector2d _myMapLocation = new Vector2d(55.8257f, 49.0538f);
        public bool _IsPlayerNear = false;


        public GameObject _playerObject;

        public class PointOfInterestEventArgs : EventArgs
        {
            public Vector2d Location;
            public bool IsPlayerNear;
            public GameObject UnityObject;
        }

        public PointOfInterestData Metadata { get; set; }

        public event EventHandler<PointOfInterestEventArgs> OnPOIClose;

        public float _myDistanceCutOff = 0.0025f;
        public float _debug_DistanceToPlayer;

        ILocationProvider _locationProvider;

        public ILocationProvider LocationProvider
        {
            private get
            {
                if (_locationProvider == null)
                {
                    _locationProvider = _useTransformLocationProvider
                        ? LocationProviderFactory.Instance.TransformLocationProvider
                        : LocationProviderFactory.Instance.DefaultLocationProvider;
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

        private GameSparksBattle _gsb;

        private void OnEnable()
        {
            if (messageRetranslator != null)
            {
                messageRetranslator.Subscribe(MessageType.POI_UPDATE, OnPOIUpdate);
            }
        }

        private void OnDisable()
        {
            if (messageRetranslator != null)
            {
                messageRetranslator.Unsubscribe(MessageType.POI_UPDATE, OnPOIUpdate);
            }
        }

        void Start()
        {
            _targetPosition = Conversions.GeoToWorldPosition(_myMapLocation,
                MapController.ReferenceTileRect.Center,
                MapController.WorldScaleFactor).ToVector3xz();
            
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            _playerObject = GameObject.FindGameObjectWithTag("Player");
            _gsb = GameObject.Find("GameSparks").GetComponent<GameSparksBattle>();
            _gsb.OnScriptMessagePOIOwnerChange += OnOwnerChange;
            GetComponent<PointColor>().UpdateView(Metadata.IsYour());
        }

        void OnDestroy()
        {
            if (LocationProvider != null)
            {
                LocationProvider.OnLocationUpdated -= LocationProvider_OnLocationUpdated;
            }
            if (_gsb != null)
            {
                _gsb.OnScriptMessagePOIOwnerChange -= OnOwnerChange;
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
            transform.position =
                Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _positionFollowFactor);
            checkDistanceToPlayerEvklid();
        }

        void checkDistanceToPlayerEvklid()
        {
            Vector2d interestLocation = _myMapLocation;
            var positionWithLocationProvider = FindObjectOfType<PositionWithLocationProvider>();
            if (positionWithLocationProvider == null) return;
            Vector2d playerLocation = positionWithLocationProvider._myCurrentLocation;
            float distanceToInterest = (float) Vector2d.Distance(interestLocation, playerLocation);
            _debug_DistanceToPlayer = distanceToInterest;

            if (OnPOIClose == null)
            {
                return;
            }

            PointOfInterestEventArgs e = new PointOfInterestEventArgs();
            e.Location = _myMapLocation;
            e.UnityObject = gameObject;

            //if player really near
            if (distanceToInterest < _myDistanceCutOff)
            {
                //if player was not near before
                if (!_IsPlayerNear)
                {
                    _IsPlayerNear = true;
                    e.IsPlayerNear = true;
                    transform.ChangeVisibility(_IsPlayerNear);
                    OnPOIClose(this, e);
                }
            }
            else //if player really NOT near
            {
                //if player was near before
                if (_IsPlayerNear)
                {
                    _IsPlayerNear = false;
                    e.IsPlayerNear = false;
                    transform.ChangeVisibility(_IsPlayerNear);
                    OnPOIClose(this, e);
                }
            } //if
        } //fn


        public void OnRay()
        {
            var infoPanel = FindObjectOfType<PointInfoPanelController>();
            infoPanel.Show(Metadata);
        }


        public void OnOwnerChange(object sender, GameSparksBattle.SCRIPT_MESSAGE_POI_OWNER_CHANGE_ev_arg arg)
        {
            if ((arg != null) && (!arg.isError) && (arg.poid == Metadata.Poid))
            {
                Metadata.Uoid = arg.newOwnerUoid;
                Metadata.DisplayName = arg.newOwnerDisplayName;
                GetComponent<PointColor>().UpdateView(Metadata.IsYour());
            }
        }

        private void OnPOIUpdate(GSData data)
        {
            if (data.GetId() == Metadata.Poid)
            {
                Metadata.SetGSData(data);
            }
        }
    }
}