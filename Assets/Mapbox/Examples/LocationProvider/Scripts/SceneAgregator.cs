using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAgregator : MonoBehaviour {

    Scene _agregatorScene;

    float _hardcodedTimeDelta = 2.0f;

    public class LoadedScene
    {
        public string _name;
        public GameObject _rootObj;
    }

    public List<LoadedScene> _LoadedScences = new List<LoadedScene>();

    // Use this for initialization
    void Awake () {
        _agregatorScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != _agregatorScene.buildIndex)
        {
            StartCoroutine(MergeScene(scene, false));
        }
    }

    public IEnumerator MergeScene(Scene scene, bool isRootEnabled)
    {
        while(true)
        {
            if ((!scene.isLoaded) || (!scene.IsValid()))
                yield return new WaitForSecondsRealtime(_hardcodedTimeDelta);
            Debug.Log("Merging scene " + scene.path + " loaded: " + scene.isLoaded + " valid: " + scene.IsValid());
            GameObject sceneRoot = scene.GetRootGameObjects()[0];
            string sceneName = scene.name;
            sceneRoot.SetActive(isRootEnabled);
            SceneManager.MergeScenes(scene, _agregatorScene);

            LoadedScene loadedScene = new LoadedScene();
            loadedScene._name = sceneName;
            loadedScene._rootObj = sceneRoot;
            _LoadedScences.Add(loadedScene);
        }
    }

    void Update()
    {
        foreach(LoadedScene ls in _LoadedScences)
        {
            Debug.Log("Scene " + ls._name + "added.");
        }
    }

    public void SetStateOfTheScene(string name, bool isActive)
    {
        foreach(LoadedScene ls in _LoadedScences)
        {
            if(ls._name.Equals(name))
            {
                ls._rootObj.SetActive(isActive);
            }
        }
    }

    public void switchToScene(string name)
    {
        foreach (LoadedScene ls in _LoadedScences)
        {
            if (ls._name.Equals(name))
            {
                ls._rootObj.SetActive(true);
            }
            else
            {
                ls._rootObj.SetActive(false);
            }
        }
    }

	
}
