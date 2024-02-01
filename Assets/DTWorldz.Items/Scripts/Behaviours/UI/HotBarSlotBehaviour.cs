using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class HotBarSlotBehaviour : ItemSlotBehaviour
    {
        [SerializeField]
        private GameObject ClearButton;
        private Button UseButton;
        private InventoryBehaviour inventoryBehaviour;

        public override void Start()
        {
            base.Start();
            UseButton = GetComponent<Button>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DragEndDropHotBar", this);
        }

        public void ClearSlot()
        {
            var icon = GetIcon();
            var quantityText = GetQuantityText();

            if (icon != null)
            {
                icon.sprite = null;
                icon.color = new Color(255, 255, 255, 0);
            }

            if (quantityText != null)
            {
                quantityText.text = String.Empty;
            }

            ItemContainerSlot = null;
            return;
        }

        internal void SetItem(InventoryBehaviour relatedInventory, BaseItemSO itemSO)
        {
            if (relatedInventory == null || itemSO == null)
            {
                ClearSlot();
                return;
            }

            var totalQuantity = relatedInventory.ItemContainer.GetTotalQuantity(itemSO);
            if (totalQuantity == 0)
            {
                ClearSlot();
                return;
            }

            ItemContainerSlot = inventoryBehaviour.ItemContainer.GetItemContainerSlot(itemSO);
            if (ItemContainerSlot == null)
            {
                ClearSlot();
                return;
            }

            inventoryBehaviour = relatedInventory;

            var icon = GetIcon();
            var quantityText = GetQuantityText();

            if (icon != null)
            {
                icon.sprite = itemSO.Icon;
                icon.color = new Color(255, 255, 255, 1);
            }

            if (quantityText != null)
            {
                quantityText.text = totalQuantity > 1 ? totalQuantity.ToString() : String.Empty;
            }
        }

        internal void SetItem(InventoryBehaviour relatedInventory, ItemContainerSlot itemContainerSlotToMap)
        {
            if (relatedInventory == null || itemContainerSlotToMap == null || itemContainerSlotToMap.ItemSO == null)
            {
                ClearSlot();
                return;
            }

            var totalQuantity = relatedInventory.ItemContainer.GetTotalQuantity(itemContainerSlotToMap.ItemSO);
            if (totalQuantity == 0)
            {
                ClearSlot();
                return;
            }

            ItemContainerSlot = itemContainerSlotToMap;
            inventoryBehaviour = relatedInventory;

            var icon = GetIcon();
            var quantityText = GetQuantityText();

            if (icon != null)
            {
                icon.sprite = ItemContainerSlot.ItemSO.Icon;
                icon.color = new Color(255, 255, 255, 1);
            }

            if (quantityText != null)
            {
                quantityText.text = totalQuantity > 1 ? totalQuantity.ToString() : String.Empty;
            }
        }

        internal void ToggleUseButton(bool v)
        {
            UseButton.enabled = v;
        }

        internal void ToggleRemoveButton(bool v)
        {
            ClearButton.SetActive(v);
        }

        internal void Refresh(InventoryBehaviour inventoryBehaviour)
        {
            if (ItemContainerSlot != null && ItemContainerSlot.ItemSO != null && inventoryBehaviour != null)
            {
                SetItem(inventoryBehaviour, ItemContainerSlot);
            }
        }

        internal void Refresh()
        {
            if (ItemContainerSlot != null && inventoryBehaviour != null)
            {
                SetItem(inventoryBehaviour, ItemContainerSlot);
            }
        }

        public void UseItem()
        {
            if (ItemContainerSlot != null && ItemContainerSlot.ItemSO != null && inventoryBehaviour != null)
            {
                var consumable = (BaseConsumableItemSO)ItemContainerSlot.ItemSO;
                consumable.Use();
                inventoryBehaviour.RemoveItem(ItemContainerSlot.ItemSO);
                SetItem(inventoryBehaviour, consumable);
            }
        }
    }
}