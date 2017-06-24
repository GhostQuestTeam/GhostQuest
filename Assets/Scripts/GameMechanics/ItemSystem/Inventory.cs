using System;
using UnityEngine;

namespace HauntedCity.GameMechanics.ItemSystem
{

    [Serializable]
    public class Inventory
    {

        [Serializable]
        public class Slot
        {
            public int Count { get; private set; }
            [SerializeField]
            public Item Item;

            public void Clear()
            {
                Item = null;
            }

            public bool IsEmpty
            {
                get { return Item == null; }
            }
        }

        private class DragedItem
        {
            public int From { get; private set; }
            public Item Item { get; private set; }

            public DragedItem(Item item, int from)
            {
                Item = item;
                From = from;
            }
        }



        [SerializeField]
        public Slot[] Slots;

        public Slot GetSlot(uint index)
        {
            return Slots[index];
        }
        
  

        public Inventory(uint size)
        {
            Slots = new Slot[size];
        }
    }
}