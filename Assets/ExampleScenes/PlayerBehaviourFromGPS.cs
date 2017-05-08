using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviourFromGPS : MonoBehaviour {

    public GameObject gpsControlObject;
    public GameObject cameraContainer;
    public GameObject controlledObject;
    public float moveSpeed = 30;
    public float _debug_DeltaGPS;

    private GPSControl gpsControl;
    private CharacterController characterController;
    private GameObject cameraObject;
    private Camera cam;

    private bool isGPSReadFirstTime = true;

    private Vector3 baseGPS;
    private Vector3 curGPS;

    // Use this for initialization
    void Start()
    {
        gpsControl = gpsControlObject.GetComponent<GPSControl>();
        characterController = cameraContainer.GetComponent<CharacterController>();
        cameraObject = cameraContainer.transform.GetChild(0).gameObject;
        cam = cameraObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        doGPSMovement();
    }

    void doGPSMovement()
    {
        float lat = gpsControl.latitude;
        float lon = gpsControl.longitude;
        float alt = gpsControl.altitude;

        if ((lat == 0) || (lon == 0)) //low error probability
        {
            return;
        }

        if (isGPSReadFirstTime)
        {
            baseGPS = new Vector3(lat, lon, alt);
            curGPS = new Vector3(lat, lon, alt);
            isGPSReadFirstTime = false;
            return;
        }

        curGPS = new Vector3(lat, lon, alt); //update current position
        float deltaGPSval = (curGPS - baseGPS).magnitude; //delta position between base and current read
        float gpsScaleFactor = 100000f;
        float deltaGPSCut = 8 / gpsScaleFactor;

        _debug_DeltaGPS = deltaGPSval * gpsScaleFactor; //for debug output

        if (deltaGPSval < deltaGPSCut) //if we moved too short - ok
        {
            controlledObject.SetActive(true);
        }
        else //else not active
        {
            controlledObject.SetActive(false);
        }

    }

}
