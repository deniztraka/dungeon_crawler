using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI.Inventory
{
    public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public static DragAndDropItem draggedItem;                                      // Item that is dragged now
        public static GameObject icon;                                                  // Icon of dragged item
        public static DragAndDropSlot sourceCell;

        public delegate void DragEvent(DragAndDropItem item);
        public static event DragEvent OnItemDragStartEvent;                             // Drag start event
        public static event DragEvent OnItemDragEndEvent;

        private static Canvas canvas;                                                   // Canvas for item drag operation
        private static string canvasName = "DragAndDropCanvas";                         // Name of canvas
        private static int canvasSortOrder = 100;                                       // Sort order for canvas

        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject(canvasName);
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = canvasSortOrder;
            }
        }

        /// <summary>
        /// Gets DaD slot which contains this item.
        /// </summary>
        /// <returns>The slot.</returns>
        public DragAndDropSlot GetSlot()
        {
            return GetComponentInParent<DragAndDropSlot>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            sourceCell = GetSlot();                                                 // Remember source cell
            draggedItem = this;

            // Create item's icon
            icon = new GameObject();
            icon.transform.SetParent(canvas.transform);
            icon.name = "Icon";
            Image myImage = GetComponent<Image>();
            myImage.raycastTarget = false;                                          // Disable icon's raycast for correct drop handling
            Image iconImage = icon.AddComponent<Image>();
            iconImage.raycastTarget = false;
            iconImage.sprite = myImage.sprite;
            RectTransform iconRect = icon.GetComponent<RectTransform>();
            // Set icon's dimensions
            RectTransform myRect = GetComponent<RectTransform>();
            iconRect.pivot = new Vector2(0.5f, 0.5f);
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);                                           		// Set as dragged item

            if (OnItemDragStartEvent != null)
            {
                OnItemDragStartEvent(this);                                         // Notify all items about drag start for raycast disabling
            }
        }

        /// <summary>
        /// Every frame on this item drag.
        /// </summary>
        /// <param name="data"></param>
        public void OnDrag(PointerEventData data)
        {
            if (icon != null)
            {
                icon.transform.position = Input.mousePosition;                          // Item's icon follows to cursor in screen pixels
            }
        }

        /// <summary>
        /// This item is dropped.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            ResetConditions();
        }

        /// <summary>
        /// Raises the disable event.
        /// </summary>
        void OnDisable()
        {
            ResetConditions();
        }

        private void ResetConditions()
        {
            if (icon != null)
            {
                Destroy(icon);                                                          // Destroy icon on item drop
            }
            if (OnItemDragEndEvent != null)
            {
                OnItemDragEndEvent(this);                                               // Notify all cells about item drag end
            }
            draggedItem = null;
            icon = null;
            sourceCell = null;

            Image myImage = GetComponent<Image>();
            myImage.raycastTarget = true;
        }

        /// <summary>
        /// Enable item's raycast.
        /// </summary>
        /// <param name="condition"> true - enable, false - disable </param>
        public void MakeRaycast(bool condition)
        {
            Image image = GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = condition;
            }
        }
    }
}