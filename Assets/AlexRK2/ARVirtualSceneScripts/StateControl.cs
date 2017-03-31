using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControl : MonoBehaviour {

	public static StateControl Instance { get; set; }

	public enum QUEST_STEP {
		START,
		FIRST_TALK_FINISHED,
		FIRST_MARKER_FOUND,
		SECOND_TALK_FINISHED,
		SECOND_MARKER_FOUND,
		THIRD_TALK_FINISHED,
		LAST_BATTLE_WON,
		LAST_BATTLE_LOST
	};

	public static QUEST_STEP currentStep;

	// Use this for initialization
	void Start () {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);
			currentStep = QUEST_STEP.START;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Update() {
		Debug.Log(currentStep);
	}

}
