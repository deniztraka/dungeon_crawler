using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Behaviours;
using DTWorldz.Items.Models;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "ItemContainer", menuName = "DTWorldz.Items/ItemContainer", order = 1)]
    public class ItemContainerSO : ScriptableObject
    {
        public List<ItemContainerSlot> ItemSlots;

        public delegate void ItemContainerEventHandler();
        public event ItemContainerEventHandler OnInventoryUpdated;

        internal void AddItem(BaseItemSO itemSO, int quantity)
        {
            var emptyItemContainerSlot = GetEmptySlot();

            if (emptyItemContainerSlot != null)
            {
                emptyItemContainerSlot.ItemSO = itemSO;
                emptyItemContainerSlot.Quantity = quantity;

                if (OnInventoryUpdated != null)
                {
                    OnInventoryUpdated.Invoke();
                }
            }
        }

        private ItemContainerSlot GetEmptySlot()
        {
            return ItemSlots.Find(itc => itc.ItemSO == null);
        }

        internal void StackItem(ItemContainerSlot itemContainerSlot, ItemBehaviour item)
        {
            var itcToUpdate = ItemSlots.Find(itc => itc == itemContainerSlot);
            itcToUpdate.Quantity += item.Quantity;
            itcToUpdate.ItemSO = item.ItemSO;

            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        internal int GetTotalQuantity(BaseItemSO itemSO)
        {
            var totalQuantity = 0;
            var sameItems = ItemSlots.FindAll(itc => itc.ItemSO != null && itc.ItemSO.Id == itemSO.Id);
            foreach (var item in sameItems)
            {
                totalQuantity += item.Quantity;
            }
            return totalQuantity;
        }

        internal ItemContainerSlot FindItemToAllowStackedOn(BaseItemSO itemSO, int quantity)
        {
            // return a slot that has the same item and stackable or an empty slot if there is any
            var sameItems = ItemSlots.FindAll(itc => itc.ItemSO != null && itc.ItemSO.Id == itemSO.Id);
            foreach (var item in sameItems)
            {
                if (item.Quantity + quantity <= itemSO.MaxStackQuantity)
                {
                    return item;
                }
            }

            var emptySlot = GetEmptySlot();
            if (emptySlot != null)
            {
                return emptySlot;
            }

            return null;
        }

        internal void RefreshList(List<ItemContainerSlot> itemContainerSlotList)
        {
            ItemSlots.Clear();
            ItemSlots.AddRange(itemContainerSlotList);
        }

        void OnValidate()
        {
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        internal void RemoveItem(BaseItemSO itemSO)
        {
            var itemSlot = ItemSlots.Find(itc => itc.ItemSO != null && itc.ItemSO.Id == itemSO.Id);
            if (itemSlot != null && itemSlot.Quantity == 1)
            {
                itemSlot.ItemSO = null;
                itemSlot.Quantity = 0;
            }
            else if (itemSlot != null && itemSlot.Quantity > 1)
            {
                itemSlot.Quantity--;
            }

            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        internal bool HasEmptySlot()
        {
            // find a slot that has no item
            return ItemSlots.Exists(itc => itc.ItemSO == null);
        }

        internal bool HasItem(BaseItemSO itemSO, int quantity)
        {
            return ItemSlots.Exists(itc => itc.ItemSO != null && itc.ItemSO.Id == itemSO.Id && itc.Quantity >= quantity);
        }

        internal ItemContainerSlot GetItemContainerSlot(int slotIndex)
        {
            return slotIndex == -1 ? null : ItemSlots[slotIndex];
        }

        internal ItemContainerSlot GetItemContainerSlot(BaseItemSO itemSO)
        {
            return ItemSlots.Find(itc => itc.ItemSO != null && itc.ItemSO.Id == itemSO.Id);
        }
    }
}
