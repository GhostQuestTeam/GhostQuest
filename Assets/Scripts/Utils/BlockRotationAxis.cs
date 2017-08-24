using UnityEngine;

namespace HauntedCity.Utils
{
    public class BlockRotationAxis:MonoBehaviour
    {
        public bool BlockX;
        public bool BlockY;
        public bool BlockZ;

        public float X;
        public float Y;
        public float Z;

        private void Update()
        {        
            var newX = BlockX ? X : transform.rotation.eulerAngles.x;
            var newY = BlockY ? Y : transform.rotation.eulerAngles.y;
            var newZ = BlockZ ? Z : transform.rotation.eulerAngles.z;
            
            
            transform.rotation = Quaternion.Euler(newX, newY, newZ);
        }

    }
}