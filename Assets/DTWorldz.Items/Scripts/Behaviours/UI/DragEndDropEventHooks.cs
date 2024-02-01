using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.UI;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using DTWorldz.ScriptableObjects.Items;
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
            //Debug.Log("Normal DragEndMessage");
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
                    itemSlotDragEnd.SetItem(draggedItem, startQuantity);
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

                    // check if switch on equipment slot
                    if(itemSlotDragStart is EquipmentSlotBehavior){
                        var oldItem = (itemSlotDragStart as EquipmentSlotBehavior).Switch(itemOnTarget);
                        if(oldItem != null){
                            itemSlotDragEnd.SetItem(oldItem, 1);
                        }
                    } else {
                        // regular switch
                        itemSlotDragEnd.SetItem(draggedItem, startQuantity);
                        itemSlotDragStart.SetItem(itemOnTarget, quantityOnTarget);
                    }
                    
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
                RelatedInventory.RefreshUI();
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

        void DropToEquipmentSlot(EquipmentSlotBehavior equipmentSlotBehavior)
        {
            var item = itemSlotDragStart.GetItem();
            var targetSlotItem = equipmentSlotBehavior.GetItem();

            if (item != null && targetSlotItem != null)
            {
                //Debug.Log("Switching items");
                var oldItemSO = equipmentSlotBehavior.Switch(item);
                if (oldItemSO != null)
                {
                    itemSlotDragStart.SetItem(oldItemSO, 1);
                }
                return;
            }

            if (item is WeaponItemSO && equipmentSlotBehavior.EquipmentType == EquipmentType.RightHand)
            {
                equipmentSlotBehavior.SetItem(item as WeaponItemSO, 1);
                itemSlotDragStart.RemoveItem();
            }
        }

        void DragEndDropHotBar(HotBarSlotBehaviour hotBarSlotBehaviour)
        {
            if (itemSlotDragStart == null)
            {
                return;
            }

            var itemContainerSlot = itemSlotDragStart.GetItemContainerSlot();
            if (itemContainerSlot != null && itemContainerSlot.ItemSO is BaseConsumableItemSO && hotBarSlotBehaviour != null && RelatedInventory != null)
            {
                hotBarSlotBehaviour.SetItem(RelatedInventory, itemContainerSlot);
            }

            if (RelatedInventory != null)
            {
                RelatedInventory.RefreshHotBar();
            }
        }

    }
}