using UnityEngine;
using UnityEngine.SceneManagement;

namespace HauntedCity.Utils
{
    public class SceneRootController:MonoBehaviour
    {
        public string SceneName;
        void OnEnable()
        {
            if (SceneManager.GetActiveScene().name != SceneName)
            {
                gameObject.SetActive(false);
            }
        }
    }
}