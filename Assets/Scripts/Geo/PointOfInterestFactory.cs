using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using HauntedCity.GameMechanics.BattleSystem;
using Zenject;
using HauntedCity.GameMechanics.Main;
using HauntedCity.Utils.Extensions;

namespace HauntedCity.Geo
{
    public class PointOfInterestFactory : MonoBehaviour
    {
        public GameObject _root;
        public HashSet<GameSparksPOIsExtraction.ExtractedPointMetadata> _points = new HashSet<GameSparksPOIsExtraction.ExtractedPointMetadata>();
        public GameObject PointOfInterestPrefab;
        public GameObject GameSparksObj;

        [Inject]
        private GameController _gameController;

        private void Awake()
        {
//            _root = new GameObject("POIRoot");
//            _root.transform.SetParent(GameObject.Find("LocationProviderRoot").transform);
        }

        void Start()
        {
        
            InitPoints();
            //Execute();
        }

        void Update()
        {
        }

        void OnEnable()
        {
            GameSparksObj.GetComponent<GameSparksPOIsExtraction>().UpdatePointsNow();
        }
        
        

        public void InitPoints()
        {
             /* _points.Add(new Vector2d(55.8254, 49.0535));
            _points.Add(new Vector2d(55.8260, 49.0532));
            _points.Add(new Vector2d(55.8265, 49.0534));

            _points.Add(new Vector2d(55.6884, 37.4629));


            _points.Add(new Vector2d(55.7469576,
                37.6427946)); //Grand Stalinist high-rise aparment bloc, built 1938-1952. One of the so-called Seven Sisters
            _points.Add(new Vector2d(55.9286013, 37.2414645)); //усадьба Середниково
            _points.Add(new Vector2d(55.756784, 37.5886476)); //Особняк Миндовского
            _points.Add(new Vector2d(55.7514667, 37.6179211)); //Царь-пушка
            _points.Add(new Vector2d(55.7508375, 37.6184908)); //Царь-колокол
            _points.Add(new Vector2d(55.7653354, 37.6054856)); //памятник А. С. Пушкину
            _points.Add(new Vector2d(55.7559664, 37.5891178)); //Городская усадьба С.С. Гагарина*/

            GameSparksPOIsExtraction ext = GameSparksObj.GetComponent<GameSparksPOIsExtraction>();
            ext.OnPOIsExtracted += Ext_OnPOIsExtracted;
        }

        public void Ext_OnPOIsExtracted(object sender, GameSparksPOIsExtraction.POIsExtractedEventArgs e)
        {
            GameSparksObj.GetComponent<GameSparksPOIsExtraction>().OnPOIsExtracted -= Ext_OnPOIsExtracted;
            _points = e.points;
            Execute();
        }

        void OnDestroy()
        {
            GameSparksObj.GetComponent<GameSparksPOIsExtraction>().OnPOIsExtracted -= Ext_OnPOIsExtracted;
        }
 
        public void Execute()
        {
            //_btnToEnable = GameObject.Find("StartBattle").GetComponent<Button>();

            //_btnToEnable = GameObject.Find("StartBattle").GetComponent<Button>();
            //_btnToEnable.gameObject.SetActive(false);

//            GameObject _prevRoot = GameObject.Find("POIRoot");
//            if (_prevRoot != null)
//                Destroy(_prevRoot);

            _root.transform.Clear();

            foreach (GameSparksPOIsExtraction.ExtractedPointMetadata pointMeta in _points)
            {
                GameObject newPOI = Instantiate(PointOfInterestPrefab, 100 * Vector3.down, Quaternion.identity,
                    _root.transform);
                newPOI.SetActive(true);
                PointOfInterestWithLocationProvider poiwtp = newPOI.GetComponent<PointOfInterestWithLocationProvider>();
                poiwtp._myMapLocation = pointMeta.LatLon;
                poiwtp._metadata = pointMeta;
            }
        }



        public void RemoveOnClick()
        {
        }

        

        public void MyLambdaSwitchEnablingMethod(GameObject obj, bool state)
        {
            obj.SetActive(state);
        }
    }
}