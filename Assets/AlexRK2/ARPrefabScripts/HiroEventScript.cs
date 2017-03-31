using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiroEventScript : MonoBehaviour {

	public Button backBtn;

	bool firstFound;

	void Start() {
		firstFound = true;
	}
		
	public void OnMarkerFound(ARMarker marker) {
		Debug.Log ("Hiro found.");
	}

	public void OnMarkerTracked(ARMarker marker) {
		Debug.Log ("Hiro tracked.");
		//if(StateControl.currentStep != StateControl.QUEST_STEP.FIRST_TALK_FINISHED) {
		//	return;
		//}
		if (!firstFound) {
			return;
		}
		StateControl.currentStep = StateControl.QUEST_STEP.FIRST_MARKER_FOUND;
		backBtn.gameObject.SetActive (true);
	}

}
