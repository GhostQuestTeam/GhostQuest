using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour {

	public GameObject Followee;
	public float speed = 0.01f;
	public Button backBtn;

	// Use this for initialization
	void Start () {
		//Followee = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (Followee != null) {
			transform.position = Vector3.MoveTowards (transform.position, Followee.transform.position, speed);
		} else {
			Debug.logger.Log (gameObject.name + ": my followee is null!!!");
		}
	}

	public void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetInstanceID () == Followee.gameObject.GetInstanceID ()) {
			StateControl.currentStep = StateControl.QUEST_STEP.LAST_BATTLE_LOST;
		} else {
			StateControl.currentStep = StateControl.QUEST_STEP.LAST_BATTLE_WON;
		}
		DestroyObject (this.gameObject);
		backBtn.gameObject.SetActive (true);
	}

	public void SetFollowee(GameObject flw) {
		Followee = flw;
	}

}
