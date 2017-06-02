using HauntedCity.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneRootController : MonoBehaviour {

    SceneAgregator _sceneAgregator;
    public string SceneName;

    [Inject]
    public void InitializeDependencies(SceneAgregator sceneAgregator)
    
    {
        _sceneAgregator = sceneAgregator;
   
    }

    // Use this for initialization
    void Start () {
        _sceneAgregator.OnAllScenesLoad += OnAllSceneLoad;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnAllSceneLoad()
    {
        StartCoroutine(DisableNotActive());
    }

    IEnumerator DisableNotActive()
    {
        while (true)
        {
            if(SceneName == "battle")
            {
                GameObject ob = GameObject.Find("map");
                if (ob != null)
                    ob.SetActive(false);
            }
            if(_sceneAgregator._currentScene != SceneName)
            {
                gameObject.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(0.03f);
        }
    }
}
