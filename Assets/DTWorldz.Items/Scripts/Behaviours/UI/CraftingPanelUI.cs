using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.Behaviours.UI
{
    public class CraftingPanelUI : MonoBehaviour
    {
        private Canvas canvas;
        public InventoryUI InventoryUI;
        public Transform SlotsContainer;
        void Start()
        {
            canvas = GetComponent<Canvas>();
            InventoryUI.OnInventoryClosed += new InventoryUI.InventoryUIHandler(InventoryClosed);
        }

        private void InventoryClosed()
        {
            Close();
        }

        public void Open()
        {
            canvas.enabled = true;
            ClearSlots();
        }

        public void Close()
        {
            canvas.enabled = false;
            ClearSlots();
        }

        public void ClearSlots()
        {
            try
            {
                for (int i = 0; i < SlotsContainer.transform.childCount; i++)
                {
                    var child = SlotsContainer.transform.GetChild(i);
                    var itemSlotBehaviour = child.GetComponent<ItemSlotBehaviour>();
                    itemSlotBehaviour.RemoveItem();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
            }
        }
    }
}