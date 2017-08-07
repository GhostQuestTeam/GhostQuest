using UnityEngine;
using UnityEngine.EventSystems;

namespace HauntedCity.Utils
{
    public class CameraClickHandler : MonoBehaviour
    {
        Camera camera;

        void Start()
        {
            camera = GetComponent<Camera>();
        }

        void Update()
        {
            Ray ray;
            
            #if UNITY_STANDALONE || UNITY_EDITOR
                if (!Input.GetMouseButtonDown(0))
                {
                    return;
                }

                ray = camera.ScreenPointToRay(Input.mousePosition);
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

            #else
                if (Input.touches.Length <= 0) return;
                Touch touch = Input.GetTouch(0);
                if (touch.phase != TouchPhase.Began) return;
               
                ray = camera.ScreenPointToRay(touch.position);
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerID))
                {
                    return;
                }    
            #endif
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject collideedObject = hit.transform.gameObject;
                collideedObject.SendMessage("OnRay");
            }
        }
    }
}