using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackHandler : MonoBehaviour {

	public void OnClickLevelHandler(int num) {
		UnityEngine.SceneManagement.SceneManager.LoadScene (num);
	}

}
