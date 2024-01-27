using System;
using System.Collections;
using System.Collections.Generic;
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
            ItemSlots.Add(new ItemContainerSlot(itemSO, quantity));

            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        internal void StackItem(ItemContainerSlot itemContainerSlot, int quantity)
        {
            var itcToUpdate = ItemSlots.Find(itc => itc == itemContainerSlot);
            itcToUpdate.Quantity += quantity;
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        internal int GetTotalQuantity(BaseItemSO itemSO)
        {
            var totalQuantity = 0;
            var sameItems = ItemSlots.FindAll(itc => itc.ItemSO.Id == itemSO.Id);
            foreach (var item in sameItems)
            {
                totalQuantity += item.Quantity;
            }
            return totalQuantity;
        }

        internal ItemContainerSlot FindItemToAllowStackedOn(BaseItemSO itemSO, int quantity)
        {
            return ItemSlots.Find(itc => itc.ItemSO.Id == itemSO.Id && itc.Quantity + quantity <= itemSO.MaxStackQuantity);
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
            var itemSlot = ItemSlots.Find(itc => itc.ItemSO.Id == itemSO.Id);
            if (itemSlot != null && itemSlot.Quantity == 1)
            {
                ItemSlots.Remove(itemSlot);
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

        internal bool HasItem(BaseItemSO itemSO, int quantity)
        {
            var itemSlot = ItemSlots.Find(itc => itc.ItemSO.Id == itemSO.Id);
            if (itemSlot != null && itemSlot.Quantity >= quantity)
            {
                return true;
            }
            return false;
        }
    }
}
