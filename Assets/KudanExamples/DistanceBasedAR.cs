using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedAR : MonoBehaviour {

    public GPSProvider gpsProvider;
    public TrivialKudanApp ka;
    public Camera cam;

    private bool isGPSReadFirstTime = true;

    private Vector3 startGPS;
    private Vector3 curGPS;

    public bool isTracking = false;
    private Vector3 trackingPosition = new Vector3(65, -20, 2000);

    public float _debug_DeltaGPS;

    // Use this for initialization
    void Start () {
        trackingPosition = cam.transform.forward * 50;
        ka.StartMarkerlessTracking(trackingPosition, Quaternion.identity);
        isTracking = true;
	}
	
	// Update is called once per frame
	void Update () {

        float lat = gpsProvider.Latitude;
        float lon = gpsProvider.Longitude;
        float alt = gpsProvider.Altitude;

        if ((lat == 0) || (lon == 0)) //low error probability
        {
            return;
        }

        if (isGPSReadFirstTime)
        {
            startGPS = new Vector3(lat, lon, alt);
            curGPS = new Vector3(lat, lon, alt);
            isGPSReadFirstTime = false;
            return;
        }

        curGPS = new Vector3(lat, lon, alt); //update current position
        float deltaGPSval = (curGPS - startGPS).magnitude; //delta position between prev and current read
        float gpsScaleFactor = 100000f;
        float deltaGPSCut = 10 / gpsScaleFactor;

        _debug_DeltaGPS = deltaGPSval * gpsScaleFactor; //for debug output

        if (deltaGPSval < deltaGPSCut) //if we moved too short - ok, show if not showing
        {
            if(!isTracking)
            {
                ka.StartMarkerlessTracking(trackingPosition, Quaternion.identity);
                isTracking = true;
            }
            return;
        }
        else //else stop tracking
        {
            ka.StopMarkerlessTracking();
            isTracking = false;
        }
        
    }//update

}//class
