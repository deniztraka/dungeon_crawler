using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Items.Behaviours
{
    public class InventoryBehaviour : MonoBehaviour
    {
        public GameObject ItemPrefab;
        public Transform SlotsContainer;
        public Transform HotBarContainer;
        public ItemContainerSO ItemContainer;
        public int MaxItemCount = 30;

        AudioManager audioManager;
        void Start()
        {
            audioManager = gameObject.GetComponent<AudioManager>();
        }

        private void Add(ItemBehaviour item)
        {
            ItemContainer.AddItem(item.ItemSO, item.Quantity);
            Destroy(item.gameObject);

            if (audioManager != null)
            {
                audioManager.Play("Loot");
            }
        }

        private void StackItem(ItemContainerSlot itemContainerSlot, ItemBehaviour item)
        {
            ItemContainer.StackItem(itemContainerSlot, item);

            Destroy(item.gameObject);

            if (audioManager != null)
            {
                audioManager.Play("Loot");
            }
        }

        public void AddItem(ItemBehaviour item)
        {
            if (ItemContainer == null)
            {
                Debug.Log("ItemContainer not set on inventory. Nothing happened.");
                return;
            }

            var hasEmptySlot = ItemContainer.HasEmptySlot();

            if (!item.ItemSO.Stackable && hasEmptySlot)
            {
                Add(item);
                RefreshHotBar();
                return;
            }
            else if (!item.ItemSO.Stackable && !hasEmptySlot)
            {
                Debug.Log("Item is not stackable and Inventory does not have any empty slot. Nothing happened.");
            }

            if (item.ItemSO.Stackable)
            {
                var itemContainerEligibleToBeStacked = ItemContainer.FindItemToAllowStackedOn(item.ItemSO, item.Quantity);
                if (itemContainerEligibleToBeStacked != null)
                {
                    StackItem(itemContainerEligibleToBeStacked, item);
                    RefreshHotBar();
                    return;
                }

                // couldn't stack but add to empty slot
                if (hasEmptySlot)
                {
                    Add(item);
                    RefreshHotBar();
                    return;
                }
            }


        }

        internal void RemoveItem(BaseItemSO itemSO)
        {
            ItemContainer.RemoveItem(itemSO);
        }

        internal void DropItem(UI.ItemSlotBehaviour itemSlotDragStart)
        {
                var player = GameManager.Instance.PlayerBehaviour;
                StartCoroutine(LateDrop(itemSlotDragStart.GetItem().Prefab, player.transform.position, 0.25f, itemSlotDragStart.GetItem(), itemSlotDragStart.GetQuantity()));
                itemSlotDragStart.RemoveItem();
                RefreshHotBar();
        }

        internal void RefreshHotBar()
        {
            if (HotBarContainer != null)
            {
                for (int i = 0; i < HotBarContainer.childCount; i++)
                {
                    var hotBarSlot = HotBarContainer.GetChild(i);
                    var hotBarSlotBehaviour = hotBarSlot.GetComponent<DTWorldz.Items.Behaviours.UI.HotBarSlotBehaviour>();
                    hotBarSlotBehaviour.Refresh(this);
                }
            }
        }

        IEnumerator LateDrop(GameObject ItemPrefab, Vector3 position, float delay, BaseItemSO itemSO, int quantity)
        {
            yield return new WaitForSeconds(delay);

            // changing item's position close to player randomly
            var randomPosition = new Vector3(position.x + UnityEngine.Random.Range(-1f, 1f), position.y + UnityEngine.Random.Range(-1f, 1f), position.z);

            var instantiatedObj = Instantiate(ItemPrefab, randomPosition, Quaternion.identity);
            var instantiatedItemBehaviour = instantiatedObj.GetComponent<ItemBehaviour>();
            instantiatedItemBehaviour.SetItem(itemSO, quantity);
        }

        internal void HasItem(BaseItemSO itemSO, int quantity, Action<bool> value)
        {
            value.Invoke(ItemContainer.HasItem(itemSO, quantity));
        }

        public void RefreshUI(){
            if (SlotsContainer != null)
            {
                for (int i = 0; i < SlotsContainer.childCount; i++)
                {
                    var child = SlotsContainer.GetChild(i);
                    var itemSlotBehaviour = child.GetComponent<DTWorldz.Items.Behaviours.UI.ItemSlotBehaviour>();
                    if (i < ItemContainer.ItemSlots.Count)
                    {
                        itemSlotBehaviour.SetItem(ItemContainer.ItemSlots[i].ItemSO, ItemContainer.ItemSlots[i].Quantity);
                    }
                    else
                    {
                        itemSlotBehaviour.RemoveItem();
                    }
                }
            }
        }
    }
}