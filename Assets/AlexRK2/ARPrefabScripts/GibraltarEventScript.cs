using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GibraltarEventScript : MonoBehaviour {

	public Button backBtn;

	bool firstFound;

	void Start() {
		firstFound = true;
	}

	public void OnMarkerFound(ARMarker marker) {
		Debug.Log ("Gibraltar found.");
	}

	public void OnMarkerTracked(ARMarker marker) {
		Debug.Log ("Gibraltar tracked.");
		if(StateControl.currentStep != StateControl.QUEST_STEP.SECOND_TALK_FINISHED) {
			return;
		}
		if (!firstFound) {
			return;
		}
		firstFound = false;
		StateControl.currentStep = StateControl.QUEST_STEP.SECOND_MARKER_FOUND;
		backBtn.gameObject.SetActive (true);
	}

}
