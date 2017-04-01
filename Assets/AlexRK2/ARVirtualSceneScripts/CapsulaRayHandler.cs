using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapsulaRayHandler : MonoBehaviour {

	public Button lvl1Btn;
	public Button lvl2Btn;
	public Button lvl3Btn;

	void Start() {
	}

	public void onRay() {
		switch (StateControl.currentStep) {
		case StateControl.QUEST_STEP.START:
			lvl1Btn.gameObject.SetActive (true);
			lvl2Btn.gameObject.SetActive (false);
			lvl3Btn.gameObject.SetActive (false);
			StateControl.currentStep = StateControl.QUEST_STEP.FIRST_TALK_FINISHED;
			break;
		case StateControl.QUEST_STEP.FIRST_MARKER_FOUND:
			lvl1Btn.gameObject.SetActive (false);
			lvl2Btn.gameObject.SetActive (true);
			lvl3Btn.gameObject.SetActive (false);
			StateControl.currentStep = StateControl.QUEST_STEP.SECOND_TALK_FINISHED;
			break;
		case StateControl.QUEST_STEP.SECOND_MARKER_FOUND:
			lvl1Btn.gameObject.SetActive (false);
			lvl2Btn.gameObject.SetActive (false);
			lvl3Btn.gameObject.SetActive (true);
			StateControl.currentStep = StateControl.QUEST_STEP.THIRD_TALK_FINISHED;
			break;
		default:
			break;
		}
	}

}
