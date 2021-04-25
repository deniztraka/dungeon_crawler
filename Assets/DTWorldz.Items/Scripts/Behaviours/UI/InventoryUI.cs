using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class InventoryUI : MonoBehaviour
    {
        public GameObject SlotPrefab;
        private ItemContainerSO itemContainer;
        // Start is called before the first frame update
        void Start()
        {
            var playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                var inventoryBehaviour = playerObject.GetComponent<InventoryBehaviour>();
                if (inventoryBehaviour != null && inventoryBehaviour.ItemContainer != null)
                {
                    itemContainer = inventoryBehaviour.ItemContainer;
                    itemContainer.OnInventoryUpdated += new ItemContainerSO.ItemContainerEventHandler(RefreshUI);
                }
            }

            RefreshUI();
        }

        public void Close()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = false;
        }

        public void Open()
        {
            var canvas = GetComponent<Canvas>();
            canvas.enabled = true;
        }

        public void RefreshUI()
        {
            try
            {


                if (gameObject == null)
                {
                    return;
                }

                var slotsContainer = transform.GetComponentInChildren<GridLayoutGroup>();
                if (itemContainer != null)
                {
                    for (int i = 0; i < slotsContainer.transform.childCount; i++)
                    {
                        var child = slotsContainer.transform.GetChild(i);
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
                    for (int i = 0; i < slotsContainer.transform.childCount; i++)
                    {
                        var child = slotsContainer.transform.GetChild(i);
                        var itemSlotBehaviour = child.GetComponent<ItemSlotBehaviour>();
                        itemSlotBehaviour.RemoveItem();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
            }
        }

        // void OnValidate()
        // {
        //     RefreshUI();
        // }
    }
}