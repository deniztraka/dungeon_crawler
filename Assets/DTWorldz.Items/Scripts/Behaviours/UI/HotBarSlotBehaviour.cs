using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class HotBarSlotBehaviour : ItemSlotBehaviour
    {
        private InventoryBehaviour inventoryBehaviour;
        public override void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DragEndDropHotBar", this);
        }

        internal void SetItem(InventoryBehaviour relatedInventory, BaseItemSO baseItemSO)
        {
            inventoryBehaviour = relatedInventory;
            this.ItemSO = baseItemSO;

            var totalQuantity = inventoryBehaviour.ItemContainer.GetTotalQuantity(baseItemSO);
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
                    Icon.sprite = baseItemSO.Icon;
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

        public void UseItem()
        {
            if (ItemSO != null)
            {
                ItemSO.Use();
            }
        }
    }
}