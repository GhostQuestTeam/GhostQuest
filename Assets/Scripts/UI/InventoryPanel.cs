using System.Collections.Generic;
using System.Linq;
using HauntedCity.GameMechanics.ItemSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HauntedCity.Utils.Extensions;

namespace HauntedCity.UI
{
    [RequireComponent(typeof(GraphicRaycaster))]
    public class InventoryPanel : MonoBehaviour, IDropHandler
    {
        public Sprite EmptySlotSprite;
        [SerializeField] public Inventory Inventory;

        private void Awake()
        {
            if (Inventory == null)
            {
                Inventory = new Inventory((uint) gameObject.GetChildren().Count);
            }
        }

        private void OnEnable()
        {
            UpdateView();
        }

        public void OnDrop(PointerEventData eventData)
        {
            var from = eventData.pointerDrag.transform.parent.gameObject;
            var raycaster = GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();

            raycaster.Raycast(eventData, results);
            results = results.Where((result) => result.gameObject.CompareTag("InventoryItem")).ToList();
            if (results.Count > 0)
            {
                var slot = results[0].gameObject;
                var name = slot.name;
                if (from.name != name)
                {
                    var fromSlot = Inventory.GetSlot(uint.Parse(from.name));
                    var destinationSlot = Inventory.GetSlot(uint.Parse(name));

                    destinationSlot.Item = fromSlot.Item;
                    fromSlot.Clear();
                }
                UpdateView();
            }
        }

        public void UpdateView()
        {
            foreach (var slotObj in gameObject.GetChildren())
            {
                var slot = Inventory.GetSlot(uint.Parse(slotObj.name));
                var sprite = slot.IsEmpty ? EmptySlotSprite : slot.Item.Picture;
                slotObj.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            }
        }
    }
}