using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIInfoWindowUnityInteractionPoint : MonoBehaviour {

    public POIInfoWindowController Controller;

	void Start () {
        Controller = new POIInfoWindowController();
        Controller.Initialize
            (
                "PointInfoWindowGrid",
                GameObject.Find("MapPanel"),
                Vector3.zero
            );
        Controller.CloseCallback = () =>
        {
            Controller.hide();
        };
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
