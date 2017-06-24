using UnityEngine;

namespace HauntedCity.GameMechanics.ItemSystem
{
    [CreateAssetMenu(fileName = "New item", menuName = "Items/Simple item")]
    public class Item:ScriptableObject
    {
        public Sprite Picture;
        public string Title;
        public string Description;
        public int Cost;
        public bool Stackable;
          
    }
}