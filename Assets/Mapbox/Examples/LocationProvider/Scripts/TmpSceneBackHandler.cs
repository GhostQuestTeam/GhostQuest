using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpSceneBackHandler : MonoBehaviour {

	public void OnBackClick()
    {
        GameObject.Find("SceneAgregator").GetComponent<SceneAgregator>().switchToScene("LocationProvider");
    }
}
