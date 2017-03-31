using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereShooter : MonoBehaviour {

	GameObject prefab;
	public GameObject cameraParent;

	// Use this for initialization
	void Start () {
		prefab = Resources.Load ("bullet") as GameObject;
	}

	public void onShootClick() {
		Camera camera = cameraParent.transform.GetChild(0).gameObject.GetComponent<Camera>();
		GameObject newBullet = Instantiate (prefab) as GameObject;
		newBullet.transform.position = cameraParent.transform.position + camera.transform.forward * 2;
		Rigidbody rb = newBullet.GetComponent<Rigidbody> ();
		rb.velocity = camera.transform.forward * 40;
	}

}
