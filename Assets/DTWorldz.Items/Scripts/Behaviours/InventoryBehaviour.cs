using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.UI.Inventory;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items.Behaviours
{
    public class InventoryBehaviour : MonoBehaviour
    {
        public Transform SlotsContainer;
        public ItemContainerSO ItemContainer;
        public int MaxItemCount = 30;

        AudioManager audioManager;
        void Start()
        {
            audioManager = gameObject.GetComponent<AudioManager>();
        }

        public void RefreshItemContainer(){
            if(SlotsContainer != null){
                var itemContainerSlotList = new List<ItemContainerSlot>();
                for (int i = 0; i < SlotsContainer.childCount; i++)
                {
                    var slot = SlotsContainer.GetChild(i);
                    var uiSlotBehaviour =  slot.GetComponent<DTWorldz.Items.Behaviours.UI.ItemSlotBehaviour>();
                    var item = uiSlotBehaviour.GetItem();
                    if(item!=null){
                        itemContainerSlotList.Add(new ItemContainerSlot(uiSlotBehaviour.GetItem(),uiSlotBehaviour.GetQuantity()));
                    }
                    
                }

                ItemContainer.RefreshList(itemContainerSlotList);
            }
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
            ItemContainer.StackItem(itemContainerSlot,item.Quantity);
            
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

            var hasEmptySlot = ItemContainer.ItemSlots.Count < MaxItemCount;

            if (!item.ItemSO.Stackable && hasEmptySlot)
            {
                Add(item);
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
                    return;
                }

                // couldn't stack but add to empty slot
                if (hasEmptySlot)
                {
                    Add(item);
                    return;
                }
            }
        }
    }
}