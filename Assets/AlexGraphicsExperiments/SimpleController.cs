using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleController : MonoBehaviour {

    public GameObject whereInstantiateAmmo;
    public GameObject ammoPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float speed = 100;
        float ammoSpeed = 250;

        if (Input.GetKey(KeyCode.UpArrow)) {
            gameObject.transform.Rotate(Vector3.left * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            gameObject.transform.Rotate(Vector3.right * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            gameObject.transform.Rotate(Vector3.down * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newAmmo = Instantiate(ammoPrefab, whereInstantiateAmmo.transform.position, whereInstantiateAmmo.transform.rotation);
            Rigidbody rb = newAmmo.GetComponent<Rigidbody>();
            Camera cam = gameObject.GetComponent<Camera>();
            rb.AddForce(cam.transform.forward * ammoSpeed);
            gameObject.transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
        }

    }//upd

}
