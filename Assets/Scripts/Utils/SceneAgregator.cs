using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HauntedCity.Utils
{
    public class SceneAgregator : MonoBehaviour
    {
        Scene _agregatorScene;

        float _hardcodedTimeDelta = 0.2f;
        private string _currentScene;
        private int _notLoadedScenes;

        public class LoadedScene
        {
            public string _name;
            public GameObject _rootObj;

            public LoadedScene(Scene scene)
            {
                _name = scene.name;
                _rootObj = scene.GetRootGameObjects()[0];
            }
        }

        public event Action OnAllScenesLoad;
        public event Action<string> OnSceneChange;

        public Dictionary<string, LoadedScene> _LoadedScences = new Dictionary<string, LoadedScene>();

        // Use this for initialization
        void Start()
        {
            _agregatorScene = SceneManager.GetActiveScene();
            _currentScene = _agregatorScene.name;
            _LoadedScences.Add(_agregatorScene.name, new LoadedScene(_agregatorScene));
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            _notLoadedScenes = 2;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
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
            while ((!scene.isLoaded) || (!scene.IsValid()))
                yield return new WaitForSecondsRealtime(_hardcodedTimeDelta);
            Debug.Log("Merging scene " + scene.path + " loaded: " + scene.isLoaded + " valid: " + scene.IsValid());

            LoadedScene loadedScene = new LoadedScene(scene);
            SceneManager.MergeScenes(scene, _agregatorScene);

            loadedScene._rootObj.SetActive(isRootEnabled);

            _LoadedScences.Add(loadedScene._name, loadedScene);
            _notLoadedScenes--;
            if (_notLoadedScenes == 0 && OnAllScenesLoad != null)
            {
                OnAllScenesLoad();
            }
        }

        void Update()
        {
        }

        public void SetStateOfTheScene(string name, bool isActive)
        {
        }

        public void switchToScene(string name)
        {
            _LoadedScences[_currentScene]._rootObj.SetActive(false);
            _LoadedScences[name]._rootObj.SetActive(true);
            _currentScene = name;
            if (OnSceneChange != null)
            {
                OnSceneChange(name);
            }
        }
    }

}
