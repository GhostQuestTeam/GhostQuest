using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HauntedCity.Utils.Extensions
{
    public static class UnityExtension
    {
        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        
        public static void ChangeVisibility(this Transform transform, bool isVisible)
        {
            if (isVisible)
            {
                transform.localScale = Vector3.one;
            }
            else
            {
                transform.localScale = Vector3.zero;
            }
        }
        
        public static List<GameObject> GetChildren(this GameObject go)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform tran in go.transform)
            {
                children.Add(tran.gameObject);
            }
            return children;
        }
    }
}