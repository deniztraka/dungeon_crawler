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
        [SerializeField]
        private GameObject ClearButton;
        private Button UseButton;
        private InventoryBehaviour inventoryBehaviour;

        void Start(){
            UseButton = GetComponent<Button>();
        }

        public override void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DragEndDropHotBar", this);
        }

        internal void SetItem(InventoryBehaviour relatedInventory, BaseItemSO baseItemSO)
        {
            if (relatedInventory == null || baseItemSO == null)
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

        public void ClearItem()
        {
            SetItem(null, null);
        }

        public void UseItem()
        {
            if (ItemSO != null && inventoryBehaviour != null)
            {
                ItemSO.Use();
                inventoryBehaviour.RemoveItem(ItemSO);
            }
        }
    }
}