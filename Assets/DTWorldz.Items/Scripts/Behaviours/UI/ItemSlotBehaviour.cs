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
        private BaseItemSO itemSO;
        private int quantity;

        private ItemSlotBehaviour dragStartedSlot;

        public bool HasItem
        {
            get
            {
                return itemSO != null;
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
        }

        internal BaseItemSO GetItem()
        {
            return itemSO;
        }

        private void OnItemDragStart(ItemSlotBehaviour itemSlotBehaviour)
        {
            dragStartedSlot = itemSlotBehaviour;
            //Debug.Log(gameObject.name + " " + itemSlotBehaviour.itemSO.name + " start");

            SendMessageUpwards("DragStartMessage", this);
        }

        private void OnItemDragEnd(ItemSlotBehaviour itemBehaviour)
        {
            //Debug.Log(gameObject.name + " " + itemBehaviour.itemSO.name + " end");

        }

        internal void SetItem(ItemContainerSlot itemContainerSlot)
        {
            itemSO = itemContainerSlot.ItemSO;
            Icon.sprite = itemSO.Icon;
            Icon.enabled = true;

            quantity = itemContainerSlot.Quantity;
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
            itemSO = null;
            QuantityText.text = String.Empty;
        }

        public void OnDrop(PointerEventData eventData)
        {
            //Debug.Log("end slot :" + this.gameObject.name + ": " + (dragStartedSlot != null ? "same" : "different"));
            SendMessageUpwards("DragEndMessage", this);
        }
    }
}