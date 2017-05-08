using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSControl : MonoBehaviour {

	public float latitude;
	public float longitude;
	public float altitude;

	public static GPSControl Instance { get; set; }


	// Use this for initialization
	void Start () {
		Instance = this;
		DontDestroyOnLoad (gameObject);
		StartCoroutine (StartGPSService ());
	}

	IEnumerator StartGPSService() {
		if (!Input.location.isEnabledByUser) {
			yield break;
		}
		while (true) {
			if ((Input.location.status != LocationServiceStatus.Running) || (Input.location.status != LocationServiceStatus.Initializing)) {
				Input.location.Start (0.1f, 0.1f);
				int maxWait = 5;
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
			}//if not running

			latitude = Input.location.lastData.latitude;
			longitude = Input.location.lastData.longitude;
			altitude = Input.location.lastData.altitude;

			yield return new WaitForEndOfFrame(); 

		}//while

	}//func

}//class
