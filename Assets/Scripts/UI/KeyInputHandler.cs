using HauntedCity.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace HauntedCity.UI
{
    public class KeyInputHandler:MonoBehaviour
    {
        [Inject] private SceneAgregator _sceneAgregator;
        
        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "map")
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    _sceneAgregator.switchToScene("start_scene");
                }
            }
        }
    }
}