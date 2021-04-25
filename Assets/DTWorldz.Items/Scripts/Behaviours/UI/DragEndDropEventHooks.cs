using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using UnityEngine;
namespace DTWorldz.Items.Behaviours.UI
{
    public class DragEndDropEventHooks : MonoBehaviour
    {
        private ItemSlotBehaviour itemSlotDragStart;
        private ItemSlotBehaviour itemSlotDragEnd;
        void DragEndMessage(ItemSlotBehaviour itemSlotBehaviour)
        {
            if (itemSlotDragStart == null)
            {
                return;
            }

            if (itemSlotDragStart == itemSlotDragEnd)
            {
                return;
            }

            itemSlotDragEnd = itemSlotBehaviour;
            //Debug.Log("dragend:" + itemSlotBehaviour.gameObject.name);

            var startQuantity = itemSlotDragStart.GetQuantity();
            var draggedItem = itemSlotDragStart.GetItem();

            // regular drop to empty slot
            if (!itemSlotDragEnd.HasItem)
            {
                itemSlotDragEnd.SetItem(new ItemContainerSlot(draggedItem, startQuantity));
                itemSlotDragStart.RemoveItem();
            }
            else
            {
                var itemOnTarget = itemSlotDragEnd.GetItem();
                var quantityOnTarget = itemSlotDragEnd.GetQuantity();

                // different item case -  switch items
                if (draggedItem != itemOnTarget)
                {
                    itemSlotDragEnd.SetItem(new ItemContainerSlot(draggedItem, startQuantity));
                    itemSlotDragStart.SetItem(new ItemContainerSlot(itemOnTarget, quantityOnTarget));
                }
                else
                {
                    // todo: same items, distribute quantites
                }
            }



            itemSlotDragStart = null;
            itemSlotDragEnd = null;
        }

        void DragStartMessage(ItemSlotBehaviour itemSlotBehaviour)
        {
            itemSlotDragStart = itemSlotBehaviour;

            if (itemSlotDragStart == null)
            {
                return;
            }
            //Debug.Log("dragstart:" + itemSlotBehaviour.gameObject.name);
        }
    }
}