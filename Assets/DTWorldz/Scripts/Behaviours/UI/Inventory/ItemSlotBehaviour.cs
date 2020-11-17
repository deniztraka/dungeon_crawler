using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
namespace DTWorldz.Behaviours.UI.Inventory
{
    public class ItemSlotBehaviour : MonoBehaviour
    {
        public bool IsSelected;

        public bool IsSelectable;

        public Sprite SelectedSprite;
        public Sprite Sprite;

        public bool HasItem;
        public GameObject SlotItemPrefab;
        //private ItemDatabase itemDatabase;

        //private InventoryBehavior InventoryBehavior;


        public delegate void SlotEventHandler();
        public event SlotEventHandler OnItemAdded;
        public event SlotEventHandler OnItemRemoved;



        // public void ToggleSelect()
        // {
        //     if (!IsSelectable)
        //     {
        //         return;
        //     }

        //     if (!HasItem)
        //     {
        //         return;
        //     }

        //     var slotItem = transform.GetComponentInChildren<SlotItemBehaviour>();

        //     IsSelected = !IsSelected;
        //     SetSelected(IsSelected);           


        // }

        // private void SetItem(Item item, GameObject slotItem)
        // {
        //     var slotItemBehaviour = slotItem.GetComponent<SlotItemBehaviour>();
        //     slotItemBehaviour.Item = item;

        //     var dbItem = InventoryBehavior.ItemDatabase.getItemByID(item.Id);

        //     slotItemBehaviour.SetUI(dbItem.Icon);



        // }

        public void DropItem()
        {
            var slotItem = transform.GetComponentInChildren<SlotItemBehaviour>();
            var playerObj = GameObject.FindWithTag("Player"); // playerın oldugu yere instantiate etçen

            StartCoroutine(LateDrop(slotItem, playerObj.transform.position, 1, 0.25f));
        }

        IEnumerator LateDrop(SlotItemBehaviour itemToDrop, Vector3 position, int count, float delay)
        {
            yield return new WaitForSeconds(delay);
            var randomPosition = new Vector3(position.x + UnityEngine.Random.Range(-0.5f, 0.5f), position.y + UnityEngine.Random.Range(-0.5f, 0.5f), position.z);
            var instantiatedLootItem = Instantiate(itemToDrop.Item.Prefab, randomPosition, Quaternion.identity);

            DestroyImmediate(itemToDrop.gameObject);
            HasItem = false;
            if (OnItemRemoved != null)
            {                
                OnItemRemoved();
            }
            InventoryBehaviour.Instance.RefreshItemsData();
        }

        internal void AddItem(BaseItemBehaviour itemBehaviour)
        {
            if (itemBehaviour != null)
            {
                var slotItemBehaviour = Instantiate(SlotItemPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform).GetComponent<SlotItemBehaviour>();
                slotItemBehaviour.SetItem(itemBehaviour);

                if (OnItemAdded != null)
                {
                    OnItemAdded();
                }
                HasItem = true;
            }
        }

        // internal GameObject AddItem(Item item)
        // {
        //     if (item != null)
        //     {
        //         if (InventoryBehavior == null)
        //         {
        //             InventoryBehavior = gameObject.GetComponentInParent<InventoryBehavior>();
        //         }
        //         var itemDatabase = InventoryBehavior.ItemDatabase;
        //         var slotWrapperPanel = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel");
        //         var slotItem = Instantiate(SlotItemPrefab, new Vector3(0, 0, 0), Quaternion.identity, slotWrapperPanel);
        //         SetItem(item, slotItem);
        //         HasItem = true;

        //         if (OnItemAdded != null)
        //         {
        //             OnItemAdded();
        //         }
        //         return slotItem;
        //     }
        //     return null;
        // }

        // internal void SetSelected(bool select)
        // {
        //     if (!IsSelectable)
        //     {
        //         return;
        //     }

        //     var slotWrapperPanelImage = transform.Find("SlotWrapperCanvas").Find("SlotWrapperPanel").GetComponent<Image>();
        //     if (!select)
        //     {
        //         slotWrapperPanelImage.sprite = Sprite;
        //     }
        //     else
        //     {
        //         slotWrapperPanelImage.sprite = SelectedSprite;
        //         InventoryBehavior.UnselectSlotExcept(this);
        //     }
        //     IsSelected = select;
        // }

        internal BaseItem GetItem()
        {
            var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
            if (slotItemBehaviour != null)
            {
                return slotItemBehaviour.Item;
            }

            return null;
        }

        // internal void SetItemAmount(int newAmount)
        // {
        //     var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
        //     slotItemBehaviour.SetItemAmount(newAmount);
        // }

        // internal void RemoveItem()
        // {
        //     HasItem = false;
        //     var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
        //     Destroy(slotItemBehaviour.gameObject);
        //     SetSelected(false);
        //     if (OnItemRemoved != null)
        //     {
        //         OnItemRemoved();
        //     }
        // }

        // internal bool UseItem()
        // {
        //     var slotItemBehaviour = transform.GetComponentInChildren<SlotItemBehaviour>();
        //     var isUsed = slotItemBehaviour.UseItem();
        //     if (isUsed)
        //     {
        //         var itemUsed = GetItem();
        //         if (itemUsed.Quantity <= 0)
        //         {
        //             RemoveItem();
        //         }
        //     }
        //     return isUsed;
        // }
    }
}