using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using DTWorldz.Scripts.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class ItemSlotBehaviour : MonoBehaviour, IDropHandler
    {

        private Image icon;
        private Text quantityText;
        private int quantity;

        private ItemSlotBehaviour dragStartedSlot;
        private InventoryBehaviour playerInventory;

        private ItemContainerSlot itemContainerSlot;

        public ItemContainerSlot ItemContainerSlot
        {
            get
            {
                return itemContainerSlot;
            }

            set
            {
                itemContainerSlot = value;
            }
        }

        public int SlotIndex = -1;

        public bool HasItem
        {
            get
            {
                return this.itemContainerSlot.ItemSO != null;
            }
        }

        public virtual int GetQuantity()
        {
            return this.itemContainerSlot.Quantity;
        }

        public Image GetIcon()
        {
            return icon;
        }

        public Text GetQuantityText()
        {
            return quantityText;
        }

        public virtual void Start()
        {
            var dragAndDropBehaviour = GetComponentInChildren<ItemSlotDragAndDrop>();
            dragAndDropBehaviour.OnItemDragStartEvent += new ItemSlotDragAndDrop.ItemSlotDragAndDropEvent(OnItemDragStart);
            dragAndDropBehaviour.OnItemDragEndEvent += new ItemSlotDragAndDrop.ItemSlotDragAndDropEvent(OnItemDragEnd);

            icon = transform.Find("Icon").GetComponent<Image>();
            quantityText = transform.Find("Text").GetComponent<Text>();
            ItemContainerSlot = GameManager.Instance.PlayerBehaviour.GetComponent<InventoryBehaviour>().ItemContainer.GetItemContainerSlot(SlotIndex);
        }

        public void ItemSlotClicked()
        {
            if (this.itemContainerSlot.ItemSO == null)
            {
                return;
            }
            if (this.itemContainerSlot.ItemSO is BaseConsumableItemSO)
            {
                var consumableItemSO = this.itemContainerSlot.ItemSO as BaseConsumableItemSO;
                if (consumableItemSO != null)
                {
                    consumableItemSO.Use();
                    var playerInventory = GameManager.Instance.PlayerBehaviour.GetComponent<InventoryBehaviour>();
                    playerInventory.RemoveItem(this.itemContainerSlot.ItemSO);
                }
            }
            else if (this.itemContainerSlot.ItemSO is BaseConstructableItemSO)
            {
                var constructableItemSO = this.itemContainerSlot.ItemSO as BaseConstructableItemSO;
                if (constructableItemSO != null)
                {
                    constructableItemSO.Construct();
                }
            }
            else if (this.itemContainerSlot.ItemSO is WeaponItemSO)
            {
                var weaponItemSO = this.itemContainerSlot.ItemSO as WeaponItemSO;
                if (weaponItemSO != null)
                {
                    weaponItemSO.Equip();
                }
            }
        }

        internal virtual BaseItemSO GetItem()
        {
            return this.itemContainerSlot.ItemSO;
        }

        internal ItemContainerSlot GetItemContainerSlot()
        {
            return this.itemContainerSlot;
        }

        public virtual void OnItemDragStart(ItemSlotBehaviour itemSlotBehaviour)
        {
            dragStartedSlot = itemSlotBehaviour;
            //Debug.Log(gameObject.name + " " + itemSlotBehaviour.ItemSO.name + " start");

            SendMessageUpwards("DragStartMessage", this);
        }

        private void OnItemDragEnd(ItemSlotBehaviour itemBehaviour)
        {
            //Debug.Log(gameObject.name + " " + itemBehaviour.itemSO.name + " end");

        }

        internal virtual void SetItem(BaseItemSO itemSO, int quantity = 1)
        {
            if (itemContainerSlot == null)
            {
                RemoveItem();
                return;
            }
            itemContainerSlot.ItemSO = itemSO;
            icon.sprite = itemSO ? itemSO.Icon : null;
            icon.enabled = itemSO != null;
            itemContainerSlot.Quantity = quantity;
            SetQuantity(quantity);
        }

        internal void SetQuantity(int value)
        {
            quantity = value;
            if (quantity > 1)
            {
                quantityText.text = quantity.ToString();
            }
            else
            {
                quantityText.text = String.Empty;
            }
            if (itemContainerSlot != null)
            {
                itemContainerSlot.Quantity = quantity;
            }
        }

        internal virtual void RemoveItem()
        {
            icon.sprite = null;
            icon.enabled = false;
            itemContainerSlot.ItemSO = null;
            if (quantityText != null)
            {
                quantityText.text = String.Empty;
            }
            itemContainerSlot.Quantity = 0;
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            //Debug.Log("end slot :" + this.gameObject.name + ": " + (dragStartedSlot != null ? "same" : "different"));
            SendMessageUpwards("DragEndMessage", this);
        }
    }
}