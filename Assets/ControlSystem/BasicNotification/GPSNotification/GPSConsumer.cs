using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSConsumer : MonoBehaviour, 
INotificationConsumer<GPSNotifier, GPSNotifier.GPSDataEventArgs> {

	public GameObject oGPSNotifier;
	GPSNotifier gpsNotifier;
	bool firstNotification = true;
	GPSNotifier.GPSDataEventArgs prevArgs;

	// Use this for initialization
	void Start () {
		gpsNotifier = oGPSNotifier.GetComponent<GPSNotifier> ();
		gpsNotifier.emitter += new ConsumerDelegate<GPSNotifier, GPSNotifier.GPSDataEventArgs> (Consume);
	}
	
	public void Consume (GPSNotifier notifier, GPSNotifier.GPSDataEventArgs args) {
		if (firstNotification) {
			prevArgs = args;
			firstNotification = false;
		}
		if (args == prevArgs) {
			return;
		}
		//here smth useful
		Debug.Log (args.Latitude + " " + args.Longitude + " " + args.Altitude + " ");
	}

}
