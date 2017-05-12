using System.Collections;
using System.Collections.Generic;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;

public class PointOfInterestFactory : MonoBehaviour {

    private GameObject _root;
    public List<Vector2d> _points = new List<Vector2d>();
    public GameObject PointOfInterestPrefab;
    public Button _btnToEnable;

	// Use this for initialization
	void Start () {
        InitPoints();
        Execute();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitPoints()
    {
        //smth
        _points.Add(new Vector2d(55.8254, 49.0535));
        _points.Add(new Vector2d(55.8260, 49.0532));
        _points.Add(new Vector2d(55.8265, 49.0534));
    }

    public void Execute()
    {
        _root = new GameObject("POIRoot");

        foreach(Vector2d point in _points)
        {
            GameObject newPOI = Instantiate(PointOfInterestPrefab, _root.transform, true);
            PointOfInterestWithLocationProvider poiwtp = newPOI.GetComponent<PointOfInterestWithLocationProvider>();
            poiwtp._myMapLocation = point;
            poiwtp.OnPOIClose += PointOfInterestWithLocationProvider_OnPOIClose;
            poiwtp._metadata = new PointOfInterestMetadata();
        }

    }

    public void PointOfInterestWithLocationProvider_OnPOIClose(object sender, PointOfInterestWithLocationProvider.PointOfInterestEventArgs e)
    {
        if(e.IsPlayerNear)
        {
            _btnToEnable.gameObject.SetActive(true);
            _btnToEnable.GetComponentInChildren<Text>().text = "Я кнопочка. Я синяя. Координаты: (" + e.Location.x.ToString() + ", " + e.Location.y.ToString() + ")";
            _btnToEnable.GetComponent<Button>().onClick.AddListener(() => MyLambdaSwitchEnablingMethod(e.UnityObject, false));
        }
        else
        {
            _btnToEnable.gameObject.SetActive(false);
            _btnToEnable.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }//handler

    public void MyLambdaSwitchEnablingMethod(GameObject obj, bool state)
    {
        obj.SetActive(state);
    }

}
