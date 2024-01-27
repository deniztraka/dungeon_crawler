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
    public class ItemSlotBehaviour : MonoBehaviour, IDropHandler
    {
        public Image Icon;
        public Text QuantityText;
        [SerializeField]
        internal BaseItemSO ItemSO;
        private int quantity;

        private ItemSlotBehaviour dragStartedSlot;
        private InventoryBehaviour playerInventory;

        public bool HasItem
        {
            get
            {
                return ItemSO != null;
            }
        }

        public int GetQuantity()
        {
            return quantity;
        }

        void Start()
        {
            var dragAndDropBehaviour = GetComponentInChildren<ItemSlotDragAndDrop>();
            dragAndDropBehaviour.OnItemDragStartEvent += new ItemSlotDragAndDrop.ItemSlotDragAndDropEvent(OnItemDragStart);
            dragAndDropBehaviour.OnItemDragEndEvent += new ItemSlotDragAndDrop.ItemSlotDragAndDropEvent(OnItemDragEnd);

            var playerObj = GameObject.FindWithTag("Player");
            playerInventory = playerObj.GetComponent<InventoryBehaviour>();
        }

        public void ItemSlotClicked()
        {
            if (ItemSO == null)
            {
                return;
            }
            if (ItemSO is BaseConsumableItemSO)
            {
                var consumableItemSO = ItemSO as BaseConsumableItemSO;
                if (consumableItemSO != null)
                {
                    consumableItemSO.Use();
                    playerInventory.RemoveItem(ItemSO);
                }
            }
            else if (ItemSO is BaseConstructableItemSO)
            {
                var constructableItemSO = ItemSO as BaseConstructableItemSO;
                if (constructableItemSO != null)
                {
                    constructableItemSO.Construct();
                }
            } else if (ItemSO is WeaponItemSO)
            {
                var weaponItemSO = ItemSO as WeaponItemSO;
                if (weaponItemSO != null)
                {
                    weaponItemSO.Equip();
                }
            }
        }

        internal BaseItemSO GetItem()
        {
            return ItemSO;
        }

        public virtual void OnItemDragStart(ItemSlotBehaviour itemSlotBehaviour)
        {
            dragStartedSlot = itemSlotBehaviour;
            Debug.Log(gameObject.name + " " + itemSlotBehaviour.ItemSO.name + " start");

            SendMessageUpwards("DragStartMessage", this);
        }

        private void OnItemDragEnd(ItemSlotBehaviour itemBehaviour)
        {
            //Debug.Log(gameObject.name + " " + itemBehaviour.itemSO.name + " end");

        }

        internal void SetItem(ItemContainerSlot itemContainerSlot)
        {
            ItemSO = itemContainerSlot.ItemSO;
            Icon.sprite = ItemSO.Icon;
            Icon.enabled = true;
            SetQuantity(itemContainerSlot.Quantity == 0 ? 1 : itemContainerSlot.Quantity);
        }

        internal void SetQuantity(int value)
        {
            quantity = value;
            if (quantity > 1)
            {
                QuantityText.text = quantity.ToString();
            }
            else
            {
                QuantityText.text = String.Empty;
            }
        }

        internal void RemoveItem()
        {
            Icon.sprite = null;
            Icon.enabled = false;
            ItemSO = null;
            if(QuantityText != null){
                QuantityText.text = String.Empty;
            }
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            //Debug.Log("end slot :" + this.gameObject.name + ": " + (dragStartedSlot != null ? "same" : "different"));
            SendMessageUpwards("DragEndMessage", this);
        }
    }
}