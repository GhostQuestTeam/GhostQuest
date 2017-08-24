using UnityEngine;

namespace HauntedCity.Utils
{
    public class MinimapCameraController:MonoBehaviour
    {

        private GameObject _mainCamera;

        private void OnEnable()
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        private void Update()
        {
            transform.rotation = Quaternion.Euler(
                90f,
                0,
                -1* _mainCamera.transform.eulerAngles.y
            );
        }
    }
}