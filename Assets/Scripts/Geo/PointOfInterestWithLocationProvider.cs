using System;
using HauntedCity.GameMechanics.Main;
using HauntedCity.UI.PointInfo;
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
        [Inject] GameController _gameController;

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

        public GameSparksPOIsExtraction.ExtractedPointMetadata _metadata;

        public event EventHandler<PointOfInterestEventArgs> OnPOIClose;

        public float _myDistanceCutOff = 0.0005f;
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

        void Start()
        {
            LocationProvider.OnLocationUpdated += LocationProvider_OnLocationUpdated;
            _playerObject = GameObject.FindGameObjectWithTag("Player");
            _gsb = GameObject.Find("GameSparks").GetComponent<GameSparksBattle>();
            _gsb.OnScriptMessagePOIOwnerChange += OnOwnerChange;
            GetComponent<PointColor>().UpdateView(_metadata.IsYour());
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
            Vector2d playerLocation = _playerObject.GetComponent<PositionWithLocationProvider>()._myCurrentLocation;
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
                    OnPOIClose(this, e);
                }
            } //if
        } //fn


        public void OnClick()
        {
            var infoPanel = FindObjectOfType<PointInfoPanelController>();
            infoPanel.Show(_metadata);
//            var panelIntrPoint = GameObject.FindObjectOfType<POIInfoWindowUnityInteractionPoint>();
//            panelIntrPoint.Controller.applyPOIMetadata(_metadata);
//            panelIntrPoint.Controller.ToFightCallback = () =>
//            {
//                _gameController.StartBattle(_metadata);
//            };
//            panelIntrPoint.Controller.CloseCallback = () =>
//            {
//                panelIntrPoint.Controller.hide();
//            };
//            panelIntrPoint.Controller.show();
        }


        public void OnOwnerChange(object sender, GameSparksBattle.SCRIPT_MESSAGE_POI_OWNER_CHANGE_ev_arg arg)
        {
            if ((arg != null) && (!arg.isError) && (arg.poid == _metadata.poid))
            {
                _metadata.uoid = arg.newOwnerUoid;
                _metadata.owner_display_name = arg.newOwnerDisplayName;
                _metadata.owner_user_name = arg.newOwnerUserName;
                GetComponent<PointColor>().UpdateView(_metadata.IsYour());
            }
        }
    }
}