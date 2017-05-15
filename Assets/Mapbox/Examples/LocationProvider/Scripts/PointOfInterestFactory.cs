using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointOfInterestFactory : MonoBehaviour
{
    private GameObject _root;
    public HashSet<Vector2d> _points = new HashSet<Vector2d>();
    public GameObject PointOfInterestPrefab;
    private Button _btnToEnable;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        InitPoints();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    // Use this for initialization
    void Start()
    {

    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
           Execute();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitPoints()
    {
        //smth
        _points.Add(new Vector2d(55.8254, 49.0535));
        _points.Add(new Vector2d(55.8260, 49.0532));
        _points.Add(new Vector2d(55.8265, 49.0534));

        _points.Add(new Vector2d(55.7469576, 37.6427946)); //Grand Stalinist high-rise aparment bloc, built 1938-1952. One of the so-called Seven Sisters
        _points.Add(new Vector2d(55.9286013, 37.2414645)); //усадьба Середниково
        _points.Add(new Vector2d(55.756784, 37.5886476)); //Особняк Миндовского
        _points.Add(new Vector2d(55.7514667, 37.6179211)); //Царь-пушка
        _points.Add(new Vector2d(55.7508375, 37.6184908)); //Царь-колокол
        _points.Add(new Vector2d(55.7653354, 37.6054856)); //памятник А. С. Пушкину
        _points.Add(new Vector2d(55.7559664, 37.5891178)); //Городская усадьба С.С. Гагарина
    }

    public void Execute()
    {
        _btnToEnable = GameObject.Find("StartBattle").GetComponent<Button>();

        //_btnToEnable.gameObject.SetActive(false);
        _root = new GameObject("POIRoot");

        foreach (Vector2d point in _points)
        {
            GameObject newPOI = Instantiate(PointOfInterestPrefab, _root.transform, true);
            PointOfInterestWithLocationProvider poiwtp = newPOI.GetComponent<PointOfInterestWithLocationProvider>();
            poiwtp._myMapLocation = point;
            poiwtp.OnPOIClose += PointOfInterestWithLocationProvider_OnPOIClose;
            poiwtp._metadata = new PointOfInterestMetadata();
        }
    }

    public void PointOfInterestWithLocationProvider_OnPOIClose(object sender,
        PointOfInterestWithLocationProvider.PointOfInterestEventArgs e)
    {
        var tmp = e.Location;
        UnityAction listener = () =>
        {
            MyLambdaSwitchEnablingMethod(e.UnityObject.transform.GetChild(0).gameObject, false);
            _btnToEnable.gameObject.SetActive(false);
            _points.Remove(tmp);
        };
        if (e.IsPlayerNear)
        {
            _btnToEnable.gameObject.SetActive(true);
            _btnToEnable.onClick.AddListener(listener);
        }
        else
        {
            _btnToEnable.gameObject.SetActive(false);
            _btnToEnable.onClick.RemoveListener(listener);
            e.UnityObject.SetActive(false);
        }
    } //handler

    public void RemoveOnClick()
    {

    }

    public void MyLambdaSwitchEnablingMethod(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }
}