using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public enum EquipmentType{
        Head,
        Chest,
        Legs,
        Feet,
        LeftHand,
        RightHand,
    }

    public class EquipmentSlotBehavior : ItemSlotBehaviour
    {

        private InventoryBehaviour inventoryBehaviour;
        public EquipmentType EquipmentType;

        public override void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DropToEquipmentSlot", this);
        }

        internal void SetItem(InventoryBehaviour relatedInventory, BaseItemSO itemSO)
        {
            if (relatedInventory == null || itemSO == null)
            {
                if (Icon != null)
                {
                    Icon.sprite = null;
                    Icon.color = new Color(255, 255, 255, 0);
                }
                if (QuantityText != null)
                {
                    QuantityText.text = String.Empty;
                }
                this.ItemSO = null;
                return;
            }

            inventoryBehaviour = relatedInventory;
            this.ItemSO = itemSO;

            var totalQuantity = inventoryBehaviour.ItemContainer.GetTotalQuantity(itemSO);
            if (totalQuantity == 0)
            {
                if (Icon != null)
                {
                    Icon.sprite = null;
                    Icon.color = new Color(255, 255, 255, 0);
                }
                if (QuantityText != null)
                {
                    QuantityText.text = String.Empty;
                }
                this.ItemSO = null;
                return;
            }
            else
            {
                if (Icon != null)
                {
                    Icon.sprite = itemSO.Icon;
                    Icon.color = new Color(255, 255, 255, 1);
                }
                if (QuantityText != null)
                {
                    QuantityText.text = totalQuantity > 1 ? totalQuantity.ToString() : String.Empty;
                }
            }
        }

        internal void Refresh(InventoryBehaviour inventoryBehaviour)
        {
            if (ItemSO != null && inventoryBehaviour != null)
            {
                SetItem(inventoryBehaviour, ItemSO);
            }
        }

        internal void Refresh()
        {
            if (ItemSO != null && inventoryBehaviour != null)
            {
                SetItem(inventoryBehaviour, ItemSO);
            }
        }
    }
}