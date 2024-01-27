using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class ItemSlotDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private GameObject DragHelperObject;
        private Image draggedItemIcon;
        private Color tempColor;
        private GameObject tempDraggingObject;
        private Image draggedIcon;
        private bool dragStarted = false;
        public Vector2 DraggingOffset = new Vector2(1, 1);

        public delegate void ItemSlotDragAndDropEvent(ItemSlotBehaviour itemBehaviour);
        public event ItemSlotDragAndDropEvent OnItemDragStartEvent;                             // Drag start event
        public event ItemSlotDragAndDropEvent OnItemDragEndEvent;

        void Start()
        {
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            dragStarted = true;
            draggedItemIcon = GetComponentInChildren<Image>();
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            tempColor = draggedItemIcon.color;
            tempDraggingObject = Instantiate(DragHelperObject, pos, Quaternion.identity);
            draggedIcon = tempDraggingObject.GetComponentInChildren<Image>();
            draggedIcon.sprite = draggedItemIcon.sprite;
            draggedIcon.raycastTarget = false;  
            draggedItemIcon.color = new Color(tempColor.r, tempColor.g, tempColor.b, 0.5f);
            
            if (OnItemDragStartEvent != null)
            {
                var itemSlotBehaviour = GetComponentInParent<ItemSlotBehaviour>();
                OnItemDragStartEvent.Invoke(itemSlotBehaviour);
            }
        }


        public void OnDrag(PointerEventData eventData)
        {
            if (!dragStarted)
            {
                return;
            }
            if (draggedIcon != null)
            {
                draggedIcon.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f) + new Vector3(DraggingOffset.x, DraggingOffset.y, 0f);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!dragStarted)
            {
                return;
            }
            draggedIcon.raycastTarget = true;  
            draggedItemIcon.color = tempColor;
            if (OnItemDragEndEvent != null)
            {
                OnItemDragEndEvent.Invoke(GetComponentInParent<ItemSlotBehaviour>());
            }
            if (tempDraggingObject != null)
            {
                Destroy(tempDraggingObject);
            }
        }
    }
}