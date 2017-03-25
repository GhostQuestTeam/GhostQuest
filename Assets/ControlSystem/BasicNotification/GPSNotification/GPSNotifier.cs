using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GPSNotifier : MonoBehaviour,
INotificationEmitter<GPSNotifier, GPSNotifier.GPSDataEventArgs>,
IImmediateResultProvider<GPSNotifier.GPSDataEventArgs>
{

	public GPSProvider gpsProvider;
	public float sendingFreq = 1;

	public class GPSDataEventArgs : EventArgs {
		public float Latitude { get; set; }
		public float Longitude { get; set; }
		public float Altitude { get; set; }
		public static bool operator ==(GPSDataEventArgs e1, GPSDataEventArgs e2) {
			if(System.Object.ReferenceEquals(e1, e2)) {
				return true;
			}
			if( ((object)e1 == null) || ((object)e2 == null) ) {
				return false;
			}
			return (e1.Latitude == e2.Latitude) && (e1.Longitude == e2.Longitude) && (e1.Altitude == e2.Altitude);
		}//==
		public static bool operator !=(GPSDataEventArgs e1, GPSDataEventArgs e2) {
			return !(e1 == e2);
		}//!=
	}


	public event ConsumerDelegate<GPSNotifier, GPSDataEventArgs> emitter;


	void OnGPSDataEventHandler(GPSDataEventArgs args) {
		if (emitter != null) {
			emitter (this, args);
		}
	}


	IEnumerator StartReadingGPSData() {
		float lat, lng, alt;

		while (true) {
			lat = gpsProvider.Latitude;
			lng = gpsProvider.Longitude;
			alt = gpsProvider.Altitude;

			if ((lat == 0) && (lng == 0) && (alt == 0)) {
				yield return new WaitForSeconds (1);
				continue;
			}

			GPSDataEventArgs eventArgs = new GPSDataEventArgs ();
			eventArgs.Latitude = lat;
			eventArgs.Longitude = lng;
			eventArgs.Altitude = alt;

			OnGPSDataEventHandler (eventArgs);

			yield return new WaitForSeconds (sendingFreq);
		}//while
	}


	void Start () {
		gpsProvider = GetComponent<GPSProvider> ();
		StartCoroutine (StartReadingGPSData ());
	}


	public GPSDataEventArgs GetData () {
		GPSDataEventArgs eventArgs = new GPSDataEventArgs ();
		eventArgs.Latitude = gpsProvider.Latitude;
		eventArgs.Longitude = gpsProvider.Longitude;
		eventArgs.Altitude = gpsProvider.Altitude;
		return eventArgs;
	}

}//class
