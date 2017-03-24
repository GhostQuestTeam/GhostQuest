using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClickHandler : MonoBehaviour {

	Camera camera;

	void Start () {
		camera = GetComponent<Camera> ();
	}
	
	void Update () {
		Ray ray;
		if (SystemInfo.deviceType == DeviceType.Handheld) {
			if (Input.touches.Length > 0) { 
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began) {
					ray = camera.ScreenPointToRay (touch.position);
				} else {
					return;
				}
			} else { 
				return;
			}
		} else { 
			if (Input.GetMouseButtonDown (0)) { 
				ray = camera.ScreenPointToRay (Input.mousePosition); 
			} else {
				return;
			}
		}

		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) {
			GameObject collideedObject = hit.transform.gameObject; 
			collideedObject.SendMessage ("onRay");
		}

	} 

}
