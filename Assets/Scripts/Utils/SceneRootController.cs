using System.Linq;
using HauntedCity.Utils.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace HauntedCity.Utils
{
    
    
    
    public class SceneRootController:MonoBehaviour
    {
        
        public GameObject[] Cameras;
        public GameObject Canvas;
        
        private void Start()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChange;
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChange;
        }

        private void OnActiveSceneChange(Scene oldScene, Scene newScene)
        {
            ChangeState(newScene.GetRootGameObjects().Contains(gameObject));
        }

        private void ChangeState(bool isOwnScene)
        {
            transform.ChangeVisibility(isOwnScene);

            Cameras.ToList().ForEach((cmr) => cmr.gameObject.SetActive(isOwnScene));
            
            transform.FindAllDescedantsOfType<EventSystem>()
                .ToList()
                .ForEach((es) => es.gameObject.SetActive(isOwnScene));
            
            Canvas.SetActive(isOwnScene);

        }

        public void Show()
        {
            ChangeState(true);
        }

        public void Hide()
        {
            ChangeState(false);
        }
    }
}