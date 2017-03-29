using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSProvider : MonoBehaviour {

	public float Latitude { get; set; }
	public float Longitude { get; set; }
	public float Altitude { get; set; }

	public static GPSProvider Instance { get; set; }

	// Use this for initialization
	void Start () {
		Instance = this;
		DontDestroyOnLoad (gameObject);
		Latitude = 0f;
		Longitude = 0f;
		Altitude = 0f;
		StartCoroutine (StartGPSService ());
	}


	IEnumerator StartGPSService() {
		if (!Input.location.isEnabledByUser) {
			yield break;
		}
		while (true) {
			//try to start location service if its not running
			if (Input.location.status != LocationServiceStatus.Running) {
				Input.location.Start ();
				int maxWait = 20;
				while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
					yield return new WaitForSeconds (1);
					maxWait--;
				}
			

				if (maxWait <= 0) {
					continue;
				}

				if (Input.location.status == LocationServiceStatus.Failed) {
					continue;
				}
			}

			Latitude = Input.location.lastData.latitude;
			Longitude = Input.location.lastData.longitude;
			Altitude = Input.location.lastData.altitude;

			yield return new WaitForEndOfFrame ();
		}//while
	}//func

}//class

