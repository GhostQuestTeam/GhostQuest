using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
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

        public event Action OnAllScenesLoad;
        public event Action<string> OnSceneChange;

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

            //У МЕНЯ НЕ РАБОТАЕТ, ПОХОЖЕ ЭТО ПЫТАЕТСЯ СРАБОТАТЬ В СОСТОЯНИИ СЦЕНЫ Loading, NO!!! РАНЬШЕ ЗДЕСЬ БЫЛИ КОСТЫЛИ С КОРУТИНОЙ!
//            scene.GetRootGameObjects()[0].SetActive(false);

            if (scene.buildIndex != 0)
            {
                scene.GetRootGameObjects()
                    .Where((obj) => obj.GetComponent<SceneRootController>() != null)
                    .ToList()
                    .ForEach((obj) => obj.GetComponent<SceneRootController>().Hide()
                    );
            }
//            FindObjectsOfType<SceneRootController>().ForEach((sceneRoot)=>sceneRoot.Hide());
            if (_notLoadedScenes == 0 && OnAllScenesLoad != null)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
                OnAllScenesLoad();
            }
        }

        public void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
        {
            
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
            SceneManager.SetActiveScene(_loadedScenes[name]);
        }
    }
}