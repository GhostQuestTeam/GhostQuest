using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Networking;
using HauntedCity.Networking.Interfaces;
using HauntedCity.Utils.Extensions;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine.UI;

namespace HauntedCity.Geo
{
    public class PointOfInterestFactory : MonoBehaviour
    {
        public GameObject _root;
        public HashSet<PointOfInterestData> _points = new HashSet<PointOfInterestData>();
        public GameObject PointOfInterestPrefab;
        public GameObject GameSparksObj;
        [Inject] private MessageRetranslator _messageRetranslator;


        
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
        
        public float UpdatePeriod = 45f;

        private WaitForSeconds _updateWait;

        [Inject] private GameController _gameController;

        [Inject] private IPOIStatsManager _poiStatsManager;

        private void Awake()
        {
            _updateWait = new WaitForSeconds(UpdatePeriod);
        }

        private void Start()
        {
//            Input.compass.enabled = true;
//            Input.location.Start();
        }

        private IEnumerator UpdatePoints()
        {


            GameObject.Find("DebugText").GetComponent<Text>().text += "\n" + CurPos;
            while (true)
            {
                _poiStatsManager.RetrievePoints(5,CurPos);
                yield return _updateWait;
            }
        }

        void OnEnable()
        {
            StartCoroutine(UpdatePoints());
//            _poiStatsManager.RetrievePoints(1, locationProviderWrapper.CurPos);
            _poiStatsManager.OnPOIsExtracted += Ext_OnPOIsExtracted;
        }

        private void OnDisable()
        {
            _poiStatsManager.OnPOIsExtracted -= Ext_OnPOIsExtracted;
        }


        public void Ext_OnPOIsExtracted(object sender, POIsExtractedEventArgs e)
        {
            _points = e.points;
            Execute();
        }


        public void Execute()
        {
            _root.transform.Clear();

            foreach (var pointMeta in _points)
            {
                GameObject newPOI = Instantiate(PointOfInterestPrefab, 100 * Vector3.down, Quaternion.identity,
                    _root.transform);//TODO use zenject factories
              
                newPOI.SetActive(true);
                PointOfInterestWithLocationProvider poiwtp = newPOI.GetComponent<PointOfInterestWithLocationProvider>();
                poiwtp._myMapLocation = pointMeta.LatLon;
                poiwtp.Metadata = pointMeta;
                poiwtp.messageRetranslator = _messageRetranslator;
            }
        }
    }
}