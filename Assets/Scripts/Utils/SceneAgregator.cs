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
        public string _currentScene;
        private int _notLoadedScenes;

        public class LoadedScene
        {
            public string _name;
            public GameObject _rootObj;

            public LoadedScene(Scene scene, string nameOfRootObj)
            {
                _name = scene.name;
                if (nameOfRootObj == null)
                {
                    _rootObj = scene.GetRootGameObjects()[0];
                }
                else
                {
                    foreach(GameObject obj in scene.GetRootGameObjects())
                    {
                        if (obj.name == nameOfRootObj)
                            _rootObj = obj;
                    }
                    if(_rootObj == null)
                        _rootObj = scene.GetRootGameObjects()[0];
                }
            }//ctr
        }

        public event Action OnAllScenesLoad;
        public event Action<string> OnSceneChange;

        public Dictionary<string, LoadedScene> _LoadedScences = new Dictionary<string, LoadedScene>();
        public Dictionary<string, string> _NamesOfRoots = new Dictionary<string, string>();

        // Use this for initialization
        void Start()
        {
            InitNamesOfRoots();
            _agregatorScene = SceneManager.GetActiveScene();
            _currentScene = _agregatorScene.name;
            _LoadedScences.Add(_agregatorScene.name, new LoadedScene(_agregatorScene, _NamesOfRoots[_agregatorScene.name]));
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            _notLoadedScenes = 2;
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }

        void InitNamesOfRoots()
        {
            _NamesOfRoots.Add("start_scene", "StartSceneRoot");
            _NamesOfRoots.Add("map", "LocationProviderRoot");
            _NamesOfRoots.Add("battle", "BattleRoot");
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

            LoadedScene loadedScene = new LoadedScene(scene, scene.name);
            //string objName = loadedScene._rootObj.name;
            SceneManager.MergeScenes(scene, _agregatorScene);

            //loadedScene._rootObj = GameObject.Find(objName);
            loadedScene._rootObj.SetActive(isRootEnabled);

            _LoadedScences.Add(loadedScene._name, loadedScene);
            _notLoadedScenes--;
            if (_notLoadedScenes == 0 && OnAllScenesLoad != null)
            {
                OnAllScenesLoad();
                //StartCoroutine(Kostul());
            }
        }

        void Update()
        {
        }

        public void SetStateOfTheScene(string name, bool isActive)
        {
        }

        public IEnumerator Kostul()
        {
            while(true)
            {
                foreach (string nameScene in _LoadedScences.Keys)
                {
                    if (nameScene.Equals(_currentScene))
                        continue;
                    _LoadedScences[nameScene]._rootObj.SetActive(false);
                }
                _LoadedScences[_currentScene]._rootObj.SetActive(true);
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        public void switchToScene(string name)
        {
            _LoadedScences[_currentScene]._rootObj.SetActive(false);
            foreach(string nameScene in _LoadedScences.Keys)
            {
                if(!nameScene.Equals(_currentScene))
                    _LoadedScences[nameScene]._rootObj.SetActive(false);
            }
            _LoadedScences[name]._rootObj.SetActive(true);
            _currentScene = name;
            if (OnSceneChange != null)
            {
                OnSceneChange(name);
            }
        }
    }
}