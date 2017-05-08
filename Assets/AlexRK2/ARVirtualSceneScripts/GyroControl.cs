using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroControl : MonoBehaviour {


	public bool gyroEnabled;
	public Gyroscope gyro;

	public GameObject cameraContainer;
	private GameObject cameraObject;
	private Camera camera;
	private Quaternion rot;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		gyroEnabled = EnableGyro ();
		cameraObject = cameraContainer.transform.GetChild (0).gameObject;
		camera = cameraObject.GetComponent<Camera> ();
        //rb = cameraContainer.GetComponent<Rigidbody>();
        //rb.freezeRotation = true;
	}

	private bool EnableGyro() {
		if (SystemInfo.supportsGyroscope) {
			gyro = Input.gyro;
			gyro.enabled = true;
			cameraContainer.transform.rotation = Quaternion.Euler (90f, 0f, 0f);
			rot = new Quaternion (0, 0, 1, 0);
			return true;
		}
		return false;
	}

	void Update () {
		if (gyroEnabled) {
			cameraObject.transform.localRotation = gyro.attitude * rot;
		}
	}


}
