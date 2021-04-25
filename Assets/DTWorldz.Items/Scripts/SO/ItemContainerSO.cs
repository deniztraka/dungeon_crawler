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
        internal void AddItem(BaseItemSO itemSO)
        {
            var itemIndex = ItemSlots.FindIndex(item => item.ItemSO.Id == itemSO.Id);
            if (itemIndex > -1 && ItemSlots[itemIndex].ItemSO.Stackable)
            {
                ItemSlots[itemIndex].Quantity += 1;
            }
            else
            {
                var itemSlotToAdd = new ItemContainerSlot(itemSO, 1);
                ItemSlots.Add(itemSlotToAdd);
            }

            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }

        void OnValidate()
        {
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }
    }
}
