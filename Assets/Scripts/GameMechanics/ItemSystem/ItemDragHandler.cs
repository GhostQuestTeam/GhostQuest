using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace HauntedCity.GameMechanics.ItemSystem
{
    public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Item Item;

        private Item _startItem;
        Vector3 startPosition;
        Transform startParent;

        #region IBeginDragHandler implementation

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startItem = Item;
            startPosition = transform.position;
            startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        #endregion

        #region IDragHandler implementation

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        #endregion

        #region IEndDragHandler implementation

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (transform.parent == startParent)
            {
                transform.position = startPosition;
            }
        }

        #endregion
    }
}