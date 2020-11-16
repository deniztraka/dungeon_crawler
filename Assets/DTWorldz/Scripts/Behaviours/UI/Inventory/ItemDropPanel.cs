using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace DTWorldz.Behaviours.UI.Inventory
{
    public class ItemDropPanel : DragAndDropSlot
    {
        public override void OnDrop(PointerEventData eventData)
        {
            if (DragAndDropItem.icon != null)
            {
                DragAndDropSlot sourceSlot = DragAndDropItem.sourceCell;                

                if (sourceSlot != null)
                {
                    InventoryBehaviour.Instance.DropItem(sourceSlot.GetComponent<ItemSlotBehaviour>());                    
                }
            }
        }
    }
}