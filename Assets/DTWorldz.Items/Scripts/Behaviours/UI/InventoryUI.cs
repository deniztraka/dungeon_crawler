using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using DTWorldz.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public delegate void InventoryUIHandler();
        public event InventoryUIHandler OnInventoryOpened;
        public event InventoryUIHandler OnInventoryClosed;
        public event InventoryUIHandler OnInventoryRefreshed;
        public GameObject SlotPrefab;
        public GridLayoutGroup SlotsContainer;
        private ItemContainerSO itemContainer;
        // Start is called before the first frame update
        void Start()
        {

            var inventoryBehaviour = GameManager.Instance.PlayerBehaviour.GetComponent<InventoryBehaviour>();
            if (inventoryBehaviour != null && inventoryBehaviour.ItemContainer != null)
            {
                itemContainer = inventoryBehaviour.ItemContainer;
                itemContainer.OnInventoryUpdated += new ItemContainerSO.ItemContainerEventHandler(RefreshUI);
            }


            RefreshUI();
        }

        public void Close()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = false;
            if (OnInventoryClosed != null)
            {
                OnInventoryClosed.Invoke();
            }
        }

        public void Open()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = true;
            if (OnInventoryOpened != null)
            {
                OnInventoryOpened.Invoke();
            }
        }

        public void RefreshUI()
        {
            try
            {


                if (gameObject == null)
                {
                    return;
                }

                if (SlotsContainer == null)
                {
                    return;
                }
                if (itemContainer != null)
                {
                    for (int i = 0; i < SlotsContainer.transform.childCount; i++)
                    {
                        var child = SlotsContainer.transform.GetChild(i);
                        var itemSlotBehaviour = child.GetComponent<ItemSlotBehaviour>();
                        if (i < itemContainer.ItemSlots.Count)
                        {
                            itemSlotBehaviour.SetItem(itemContainer.ItemSlots[i]);
                        }
                        else
                        {
                            itemSlotBehaviour.RemoveItem();
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < SlotsContainer.transform.childCount; i++)
                    {
                        var child = SlotsContainer.transform.GetChild(i);
                        var itemSlotBehaviour = child.GetComponent<ItemSlotBehaviour>();
                        itemSlotBehaviour.RemoveItem();
                    }
                }

                if (OnInventoryRefreshed != null)
                {
                    OnInventoryRefreshed.Invoke();
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
            }
        }

        // void OnValidate()
        // {
        //     RefreshUI();
        // }
    }
}