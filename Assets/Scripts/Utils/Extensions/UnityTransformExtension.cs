using UnityEngine;

namespace HauntedCity.Utils.Extensions
{
    public static class UnityTransformExtension
    {
        public static void Clear(this Transform transform)
        {
            foreach (Transform child in transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }
}