﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPlaneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (WebCamTexture.devices.Length <= 0) {
			return;
		}
		WebCamTexture deviceCameraTexture = new WebCamTexture ();
		gameObject.GetComponent<Renderer> ().material.mainTexture = deviceCameraTexture;
		deviceCameraTexture.Play ();
	}

}
