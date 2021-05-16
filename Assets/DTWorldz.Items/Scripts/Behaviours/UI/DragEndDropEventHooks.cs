using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.UI;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items.Behaviours.UI
{
    public class DragEndDropEventHooks : MonoBehaviour
    {

        public InventoryBehaviour RelatedInventory;
        public StackingMenu StackingMenu;
        private ItemSlotBehaviour itemSlotDragStart;
        private ItemSlotBehaviour itemSlotDragEnd;
        void DragEndMessage(ItemSlotBehaviour itemSlotBehaviour)
        {
            Debug.Log("Normal DragEndMessage");
            if (itemSlotDragStart == null)
            {
                return;
            }

            if (itemSlotDragStart == itemSlotDragEnd)
            {
                return;
            }

            itemSlotDragEnd = itemSlotBehaviour;

            var startQuantity = itemSlotDragStart.GetQuantity();
            var draggedItem = itemSlotDragStart.GetItem();

            // regular drop to empty slot
            if (!itemSlotDragEnd.HasItem)
            {
                // if starting quantity more than one, show stack/unstack menu
                if (StackingMenu != null && startQuantity > 1)
                {
                    StackingMenu.OpenStackingMenu(itemSlotDragStart, itemSlotDragEnd);

                }
                else // just move to annother slot 1 quantity item
                {
                    itemSlotDragEnd.SetItem(new ItemContainerSlot(draggedItem, startQuantity));
                    itemSlotDragStart.RemoveItem();
                }
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
                    //same items, distribute quantites

                    // total quantity is not above max quantity
                    if (quantityOnTarget + startQuantity <= itemOnTarget.MaxStackQuantity)
                    {
                        itemSlotDragEnd.SetQuantity(quantityOnTarget + startQuantity);
                        itemSlotDragStart.RemoveItem();
                    }
                    else
                    {
                        // total quantity is above max quantity
                        var maxQuantityPossible = itemOnTarget.MaxStackQuantity;
                        var finalQuantityOnStart = quantityOnTarget + startQuantity - maxQuantityPossible;
                        itemSlotDragStart.SetQuantity(finalQuantityOnStart);
                        itemSlotDragEnd.SetQuantity(quantityOnTarget + startQuantity - finalQuantityOnStart);
                    }
                }
            }

            if (RelatedInventory != null)
            {
                RelatedInventory.RefreshItemContainer();
                RelatedInventory.RefreshHotBar();
            }

            itemSlotDragStart = null;
            itemSlotDragEnd = null;
        }

        void HotBarDragStartMessage(ItemSlotBehaviour itemSlotBehaviour)
        {
            // itemSlotDragStart = itemSlotBehaviour;

            // if (itemSlotDragStart == null)
            // {
            //     return;
            // }
        }

        void DragStartMessage(ItemSlotBehaviour itemSlotBehaviour)
        {
            itemSlotDragStart = itemSlotBehaviour;

            if (itemSlotDragStart == null)
            {
                return;
            }
        }

        void DragEndDropItem()
        {
            if (RelatedInventory && itemSlotDragStart)
            {
                RelatedInventory.RefreshHotBar();
                RelatedInventory.DropItem(itemSlotDragStart);
            }
        }

        void DragEndDropHotBar(HotBarSlotBehaviour hotBarSlotBehaviour)
        {
            Debug.Log("Hotbar DragEndMessage");

            if (itemSlotDragStart == null)
            {
                return;
            }

            var itemToSet = itemSlotDragStart.GetItem() as BaseConsumableItemSO;
            if (itemToSet != null && itemToSet.ItemType == ItemType.Consumable && hotBarSlotBehaviour != null && RelatedInventory != null && itemSlotDragStart != null)
            {
                hotBarSlotBehaviour.SetItem(RelatedInventory, itemToSet);
            }

            if (RelatedInventory != null)
            {
                RelatedInventory.RefreshHotBar();
            }
        }

    }
}