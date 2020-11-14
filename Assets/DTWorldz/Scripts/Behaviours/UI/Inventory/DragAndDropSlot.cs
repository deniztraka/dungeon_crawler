using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace DTWorldz.Behaviours.UI.Inventory
{

    /// <summary>
    /// Every item's cell must contain this script
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class DragAndDropSlot : MonoBehaviour, IDropHandler
    {
        public enum CellType                                                    // Cell types
        {
            Swap,                                                               // Items will be swapped between any cells
            DropOnly,                                                           // Item will be dropped into cell
            DragOnly                                                            // Item will be dragged from this cell
        }

        public enum TriggerType                                                 // Types of drag and drop events
        {
            DropRequest,                                                        // Request for item drop from one cell to another
            DropEventEnd,                                                       // Drop event completed
            ItemAdded,                                                          // Item manualy added into cell
            ItemWillBeDestroyed                                                 // Called just before item will be destroyed
        }

        public class DropEventDescriptor                                        // Info about item's drop event
        {
            public TriggerType triggerType;                                     // Type of drag and drop trigger
            public DragAndDropSlot sourceCell;                                  // From this cell item was dragged
            public DragAndDropSlot destinationCell;                             // Into this cell item was dropped
            public DragAndDropItem item;                                        // Dropped item
            public bool permission;                                             // Decision need to be made on request
        }

        [Tooltip("Functional type of this cell")]
        public CellType cellType = CellType.Swap;                               // Special type of this cell
        [Tooltip("Sprite color for empty cell")]
        public Color empty = new Color();                                       // Sprite color for empty cell
        [Tooltip("Sprite color for filled cell")]
        public Color full = new Color();                                        // Sprite color for filled cell
        [Tooltip("This cell has unlimited amount of items")]
        public bool unlimitedSource = false;                                    // Item from this cell will be cloned on drag start

        private DragAndDropItem myDadItem;                                      // Item of this DaD cell

        void Awake()
        {
            var itemSlotBehaviour = GetComponent<ItemSlotBehaviour>();
            if (itemSlotBehaviour)
            {
                itemSlotBehaviour.OnItemAdded += new ItemSlotBehaviour.SlotEventHandler(UpdateState);
                itemSlotBehaviour.OnItemRemoved += new ItemSlotBehaviour.SlotEventHandler(UpdateState);
            }
        }

        void UpdateState()
        {
            UpdateMyItem();
            UpdateBackgroundState();
        }

        void OnEnable()
        {
            DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;         // Handle any item drag start
            DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;             // Handle any item drag end
            UpdateMyItem();
            UpdateBackgroundState();
        }

        void OnDisable()
        {
            DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
            DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
            StopAllCoroutines();                                                // Stop all coroutines if there is any
        }

        /// <summary>
        /// On any item drag start need to disable all items raycast for correct drop operation
        /// </summary>
        /// <param name="item"> dragged item </param>
        private void OnAnyItemDragStart(DragAndDropItem item)
        {
            UpdateMyItem();
            if (myDadItem != null)
            {
                myDadItem.MakeRaycast(false);                                   // Disable item's raycast for correct drop handling
                // if (myDadItem == item)                                          // If item dragged from this cell
                // {
                //     // Check cell's type
                //     switch (cellType)
                //     {
                //         case CellType.DropOnly:
                //             DragAndDropItem.icon.SetActive(false);              // Item can not be dragged. Hide icon
                //             break;
                //     }
                // }
            }
        }

        /// <summary>
        /// On any item drag end enable all items raycast
        /// </summary>
        /// <param name="item"> dragged item </param>
        private void OnAnyItemDragEnd(DragAndDropItem item)
        {
            UpdateMyItem();
            if (myDadItem != null)
            {
                myDadItem.MakeRaycast(true);                                    // Enable item's raycast
            }
            UpdateBackgroundState();
        }

        /// <summary>
        /// Item is dropped in this cell
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnDrop(PointerEventData data)
        {
            if (DragAndDropItem.icon != null)
            {
                DragAndDropItem item = DragAndDropItem.draggedItem;
                DragAndDropSlot sourceCell = DragAndDropItem.sourceCell;
                if (DragAndDropItem.icon.activeSelf == true)                    // If icon inactive do not need to drop item into cell
                {
                    if ((item != null) && (sourceCell != this))
                    {
                        DropEventDescriptor desc = new DropEventDescriptor();

                        // Fill event descriptor
                        desc.item = item;
                        desc.sourceCell = sourceCell;
                        desc.destinationCell = this;
                        SendRequest(desc);                      // Send drop request
                        StartCoroutine(NotifyOnDragEnd(desc));  // Send notification after drop will be finished
                        if (desc.permission == true)            // If drop permitted by application
                        {
                            if (myDadItem != null)            // If destination cell has item
                            {
                                // Fill event descriptor
                                DropEventDescriptor descAutoswap = new DropEventDescriptor();
                                descAutoswap.item = myDadItem;
                                descAutoswap.sourceCell = this;
                                descAutoswap.destinationCell = sourceCell;
                                SendRequest(descAutoswap);                      // Send drop request
                                StartCoroutine(NotifyOnDragEnd(descAutoswap));  // Send notification after drop will be finished
                                if (descAutoswap.permission == true)            // If drop permitted by application
                                {
                                    SwapItems(sourceCell, this);                // Swap items between cells
                                }
                                else
                                {
                                    PlaceItem(item);            // Delete old item and place dropped item into this cell
                                }
                            }
                            else
                            {
                                PlaceItem(item);                // Place dropped item into this empty cell
                            }
                        }

                    }
                }


                if (item != null)
                {
                    if (item.GetComponentInParent<DragAndDropSlot>() == null)   // If item have no cell after drop
                    {
                        Destroy(item.gameObject);                               // Destroy it
                    }
                }
                if (gameObject != null)
                {

                    UpdateMyItem();
                    UpdateBackgroundState();

                }
                sourceCell.UpdateMyItem();
                sourceCell.UpdateBackgroundState();
            }
        }

        /// <summary>
        /// Put item into this cell.
        /// </summary>
        /// <param name="item">Item.</param>
        private void PlaceItem(DragAndDropItem item)
        {
            if (item != null)
            {
                DestroyItem();                                              // Remove current item from this cell
                myDadItem = null;
                DragAndDropSlot cell = item.GetComponentInParent<DragAndDropSlot>();
                if (cell != null)
                {
                    if (cell.unlimitedSource == true)
                    {
                        string itemName = item.name;
                        item = Instantiate(item);
                        item.transform.SetParent(transform, false);                              // Clone item from source cell
                        item.name = itemName;
                    }
                }
                item.transform.SetParent(transform, false);
                item.transform.localPosition = Vector3.zero;
                item.MakeRaycast(true);
                myDadItem = item;
            }
            UpdateBackgroundState();
        }

        private void UpdateSlotBehaviour()
        {
            var slotBehaviour = GetComponent<ItemSlotBehaviour>();
            var slotItem = transform.Find("SlotItem");
            slotBehaviour.HasItem = slotItem != null;
        }

        /// <summary>
        /// Destroy item in this cell
        /// </summary>
        private void DestroyItem()
        {
            UpdateMyItem();
            if (myDadItem != null)
            {
                DropEventDescriptor desc = new DropEventDescriptor();
                // Fill event descriptor
                desc.triggerType = TriggerType.ItemWillBeDestroyed;
                desc.item = myDadItem;
                desc.sourceCell = this;
                desc.destinationCell = this;
                SendNotification(desc);                                         // Notify application about item destruction
                if (myDadItem != null)
                {
                    Destroy(myDadItem.gameObject);
                }
            }
            myDadItem = null;
            UpdateBackgroundState();
        }

        /// <summary>
        /// Send drag and drop information to application
        /// </summary>
        /// <param name="desc"> drag and drop event descriptor </param>
        private void SendNotification(DropEventDescriptor desc)
        {
            if (desc != null)
            {
                // Send message with DragAndDrop info to parents GameObjects
                gameObject.SendMessageUpwards("OnSimpleDragAndDropEvent", desc, SendMessageOptions.DontRequireReceiver);
            }
        }

        /// <summary>
        /// Send drag and drop request to application
        /// </summary>
        /// <param name="desc"> drag and drop event descriptor </param>
        /// <returns> result from desc.permission </returns>
        private bool SendRequest(DropEventDescriptor desc)
        {
            bool result = false;
            if (desc != null)
            {
                desc.triggerType = TriggerType.DropRequest;
                desc.permission = true;
                SendNotification(desc);
                result = desc.permission;
            }
            return result;
        }

        /// <summary>
        /// Wait for event end and send notification to application
        /// </summary>
        /// <param name="desc"> drag and drop event descriptor </param>
        /// <returns></returns>
        private IEnumerator NotifyOnDragEnd(DropEventDescriptor desc)
        {
            // Wait end of drag operation
            while (DragAndDropItem.draggedItem != null)
            {
                yield return new WaitForEndOfFrame();
            }
            desc.triggerType = TriggerType.DropEventEnd;

            SendNotification(desc);


        }

        /// <summary>
        /// Change cell's sprite color on item put/remove.
        /// </summary>
        /// <param name="condition"> true - filled, false - empty </param>
        public void UpdateBackgroundState()
        {
            Image bg = GetComponent<Image>();
            if (bg != null)
            {
                bg.color = myDadItem != null ? full : empty;
            }
        }

        /// <summary>
        /// Updates my item
        /// </summary>
        public void UpdateMyItem()
        {
            myDadItem = GetComponentInChildren<DragAndDropItem>();
        }

        /// <summary>
        /// Get item from this cell
        /// </summary>
        /// <returns> Item </returns>
        public DragAndDropItem GetItem()
        {
            return myDadItem;
        }

        /// <summary>
        /// Manualy add item into this cell
        /// </summary>
        /// <param name="newItem"> New item </param>
        public void AddItem(DragAndDropItem newItem)
        {
            if (newItem != null)
            {
                PlaceItem(newItem);
                DropEventDescriptor desc = new DropEventDescriptor();
                // Fill event descriptor
                desc.triggerType = TriggerType.ItemAdded;
                desc.item = newItem;
                desc.sourceCell = this;
                desc.destinationCell = this;
                SendNotification(desc);
            }
        }

        /// <summary>
        /// Manualy delete item from this cell
        /// </summary>
        public void RemoveItem()
        {
            DestroyItem();
        }

        /// <summary>
        /// Swap items between two cells
        /// </summary>
        /// <param name="firstCell"> Cell </param>
        /// <param name="secondCell"> Cell </param>
        public SwapType SwapItems(DragAndDropSlot firstCell, DragAndDropSlot secondCell)
        {
            var swapType = SwapType.None;

            if ((firstCell != null) && (secondCell != null))
            {
                DragAndDropItem firstDragAndDropItem = firstCell.GetItem();                // Get item from first cell
                DragAndDropItem secondDragAndDropItem = secondCell.GetItem();              // Get item from second cell

                // //try stack
                // var isStacked = false;
                // if (firstDragAndDropItem != null && secondDragAndDropItem != null)
                // {
                //     var firstItemBehaviour = firstDragAndDropItem.GetComponent<SlotItemBehaviour>();
                //     var secondItemBehaviour = secondDragAndDropItem.GetComponent<SlotItemBehaviour>();
                //     if (firstItemBehaviour != null && secondItemBehaviour != null)
                //     {
                //         if (firstItemBehaviour.Item.Id.Equals(secondItemBehaviour.Item.Id))
                //         {
                //             if (secondItemBehaviour.Item.Quantity < secondItemBehaviour.Item.MaxStack)
                //             {
                //                 //requirements match for stack we can stack here 
                //                 var stackableAmount = secondItemBehaviour.Item.MaxStack - secondItemBehaviour.Item.Quantity;

                //                 if (firstItemBehaviour.Item.Quantity <= stackableAmount)
                //                 {
                //                     // all of them will be stacked to second cell
                //                     var inventoryBehaviour = secondItemBehaviour.GetComponentInParent<InventoryBehavior>();
                //                     var itemToStack = inventoryBehaviour.ItemDatabase.getItemByID(firstItemBehaviour.Item.Id);
                //                     itemToStack.Quantity = firstItemBehaviour.Item.Quantity;
                //                     secondItemBehaviour.Stack(itemToStack);



                //                     //remove first Item slot
                //                     var firstItemSlotBehaviour = firstItemBehaviour.GetComponentInParent<ItemSlotBehaviour>();
                //                     firstItemSlotBehaviour.RemoveItem();

                //                     swapType = SwapType.FullyStacked;

                //                 }
                //                 else
                //                 {
                //                     var initialFirstAmount = firstItemBehaviour.Item.Quantity;
                //                     var initialSecondAmount = secondItemBehaviour.Item.Quantity;

                //                     secondItemBehaviour.Item.Quantity = firstItemBehaviour.Item.MaxStack;
                //                     firstItemBehaviour.Item.Quantity = initialSecondAmount - (secondItemBehaviour.Item.MaxStack - initialFirstAmount);

                //                     firstItemBehaviour.SetItemAmount(firstItemBehaviour.Item.Quantity);
                //                     secondItemBehaviour.SetItemAmount(secondItemBehaviour.Item.Quantity);

                //                     if (firstItemBehaviour.Item.Quantity == 0)
                //                     {
                //                         var firstItemSlotBehaviour = firstItemBehaviour.GetComponentInParent<ItemSlotBehaviour>();
                //                         firstItemSlotBehaviour.RemoveItem();
                //                     }

                //                     swapType = SwapType.PartiallyStacked;
                //                 }


                //                 isStacked = true;
                //                 //stacking process is done here
                //             }
                //         }
                //     }
                // }

                // if (!isStacked)
                // {
                // Swap items
                if (firstDragAndDropItem != null)
                {
                    firstDragAndDropItem.transform.SetParent(secondCell.transform, false);
                    firstDragAndDropItem.transform.localPosition = Vector3.zero;
                    firstDragAndDropItem.MakeRaycast(true);
                }
                if (secondDragAndDropItem != null)
                {
                    secondDragAndDropItem.transform.SetParent(firstCell.transform, false);
                    secondDragAndDropItem.transform.localPosition = Vector3.zero;
                    secondDragAndDropItem.MakeRaycast(true);
                }

                swapType = SwapType.Swapped;
                // }

                firstCell.UpdateMyItem();
                firstCell.UpdateBackgroundState();

                secondCell.UpdateMyItem();
                secondCell.UpdateBackgroundState();

            }
            return swapType;
        }
    }

    public enum SwapType
    {
        None,
        FullyStacked,
        PartiallyStacked,
        Swapped
    }
}