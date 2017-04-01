using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraController : MonoBehaviour {

	Camera cam;

	// Use this for initialization
	void Start () {
		cam = GetComponent<Camera> ();
		makeFrontCamera (cam);
	}
	
	// Update is called once per frame
	void Update () {
		//Camera[] allCameras = new Camera[Camera.allCamerasCount];
		//Camera.GetAllCameras(allCameras);
		//for(int i = 0; i < Camera.allCamerasCount; i++) {
		//	Debug.Log(allCameras [i].ToString() + " ___ " + allCameras [i].depth);
		//}
	}

	void makeFrontCamera(Camera cam) {
		Camera[] allCameras = Camera.allCameras;
		for(int i = 0; i < Camera.allCamerasCount; i++) {
			allCameras [i].depth++;
		}
		cam.depth = 0;
	}

}
