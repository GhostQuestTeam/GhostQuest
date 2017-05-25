using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
NOT USED
*/
public class SimpleLocationProviderCacher : MonoBehaviour {

    public int _mySceneBuildIndex = 0;
    private static SimpleLocationProviderCacher _instance;

    void Awake()
    {
        if(_instance == null)
        {
            DontDestroyOnLoad(transform.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == _mySceneBuildIndex)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        if (scene.buildIndex == _mySceneBuildIndex)
        {
            GameObject mapRoot = GameObject.Find("worldRoot");
            GameObject sceneRoot = transform.GetChild(0).gameObject;
            mapRoot.transform.SetParent(sceneRoot.transform);
            sceneRoot.SetActive(false);
        }
    }
}
