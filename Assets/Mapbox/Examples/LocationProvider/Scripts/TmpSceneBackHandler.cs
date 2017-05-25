using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpSceneBackHandler : MonoBehaviour {

	public void OnBackClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
