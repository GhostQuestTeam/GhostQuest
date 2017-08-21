using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HauntedCity.Utils
{
    public class LocationProviderCacheController:MonoBehaviour
    {
        private Vector3 _defaultPosition;
        private Vector3 _anotherPosition = 1000000f * Vector3.back;

        void Awake()
        {
            _defaultPosition = transform.position;
            transform.position = _anotherPosition;
            transform.ChangeVisibility(false);
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            transform.ChangeVisibility(newScene.name == "map");
        }
        
        
    }
}