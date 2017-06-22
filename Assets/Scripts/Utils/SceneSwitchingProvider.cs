using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchingProvider : MonoBehaviour {


    private static SceneSwitchingProvider _singletonInstance = null;

    public SceneSwitchingProvider Instance
    {
        get
        {
            return _singletonInstance;
        }
    }

    Scene _firstScene;

    float _hardcodedTimeDelta = 0.2f;
    public string _currentSceneName;
    private int _notLoadedScenes;

    public class LoadedScene
    {
        public string _name;
        public Scene _scene;

        public LoadedScene(Scene scene)
        {
            _name = scene.name;
            _scene = scene;
        }
    }

    public event Action OnAllScenesLoad;
    public event Action<string> OnSceneChange;
    public event Action<string> OnSceneFailedToChange;

    public Dictionary<string, LoadedScene> _LoadedScences = new Dictionary<string, LoadedScene>();

    void Awake()
    {
        if((_singletonInstance != null) && (_singletonInstance != this))
        {
            Destroy(this.gameObject);
        }
        _singletonInstance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        _firstScene = SceneManager.GetActiveScene();
        _currentSceneName = _firstScene.name;
        _LoadedScences.Add(_firstScene.name, new LoadedScene(_firstScene));
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        _notLoadedScenes = SceneManager.sceneCountInBuildSettings - 1;
        StartLoadingScenesToIndex(_notLoadedScenes);
    }

    //first scene with build index 0 is loaded
    public void StartLoadingScenesToIndex(int lim)
    {
        for(int i = 1; i <= lim; ++i)
        {
            SceneManager.LoadSceneAsync(i, LoadSceneMode.Additive);
        }
    }

    public void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != _firstScene.buildIndex)
        {
            StartCoroutine(LoadScene(scene));
        }
    }

    public IEnumerator LoadScene(Scene scene)
    {
        while ((!scene.isLoaded) || (!scene.IsValid()))
            yield return new WaitForSecondsRealtime(_hardcodedTimeDelta);
        Debug.Log("Adding scene " + scene.path + " loaded: " + scene.isLoaded + " valid: " + scene.IsValid());

        LoadedScene loadedScene = new LoadedScene(scene);
        _LoadedScences.Add(loadedScene._name, loadedScene);

        _notLoadedScenes--;
        if (_notLoadedScenes == 0 && OnAllScenesLoad != null)
        {
            OnAllScenesLoad();
        }
       
    }

    public void switchToScene(string name)
    {
        if (name == SceneManager.GetActiveScene().name)
        {
            if(OnSceneFailedToChange != null)
                OnSceneFailedToChange(name);
            return;
        }

        LoadedScene ldScene = _LoadedScences[name];

        if(ldScene != null)
        {
            SceneManager.SetActiveScene(ldScene._scene);
            _currentSceneName = name;
            if (OnSceneChange != null)
                OnSceneChange(name);
        }
        else
        {
            if (OnSceneFailedToChange != null)
                OnSceneFailedToChange(name);
        }

        
    }//switch

}
