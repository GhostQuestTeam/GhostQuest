using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mapbox.Unity.Location;
using Mapbox.Utils;

public class TestDeviceLocation : MonoBehaviour {

    public Text locText;
    private ILocationProvider lp;

    // Use this for initialization
    void Start () {
        LocationProviderFactory lpf = gameObject.GetComponent<LocationProviderFactory>();
        lp = lpf.DefaultLocationProvider;
	}
	
	// Update is called once per frame
	void Update () {
        Vector2d pos = lp.Location;
        locText.text = "Lat: " + pos.x + "  Lon: " + pos.y;
	}
}
