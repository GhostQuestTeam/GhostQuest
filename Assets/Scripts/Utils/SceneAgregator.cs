using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HauntedCity.Utils
{
    public class SceneAgregator : MonoBehaviour
    {

        float _hardcodedTimeDelta = 0.2f;
        public string[] ScenesToLoad;
        
        private int _notLoadedScenes;
        private readonly Dictionary<string, Scene> _loadedScenes = new Dictionary<string, Scene>();
        
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

        //public Dictionary<string, LoadedScene> _LoadedScences = new Dictionary<string, LoadedScene>();

        // Use this for initialization
        void Start()
        {
            var currentScene = SceneManager.GetActiveScene();
            _loadedScenes.Add(currentScene.name, currentScene);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            
            _notLoadedScenes = ScenesToLoad.Length;
            foreach (var sceneName in ScenesToLoad)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }

        public void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _loadedScenes.Add(scene.name, scene);
            _notLoadedScenes--;
            scene.GetRootGameObjects()[0].SetActive(false);
            if (_notLoadedScenes == 0 && OnAllScenesLoad != null)
            {
                OnAllScenesLoad();
            }
        }

        public void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
        {
            oldScene.GetRootGameObjects()[0].SetActive(false);
            newScene.GetRootGameObjects()[0].SetActive(true);
            if (OnSceneChange != null)
            {
                OnSceneChange(newScene.name);
            }
        }


        void Update()
        {
        }

        void OnDestroy()
        {
            SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
            SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        }

        public void switchToScene(string name)
        {
            if (!_loadedScenes.ContainsKey(name))
            {
                throw new ArgumentException("Scene with name " + name + " not loaded");
            }
            SceneManager.GetActiveScene().GetRootGameObjects()[0].SetActive(false);
            SceneManager.SetActiveScene(_loadedScenes[name]);
            _loadedScenes[name].GetRootGameObjects()[0].SetActive(true);
            
            
        }
    }
}