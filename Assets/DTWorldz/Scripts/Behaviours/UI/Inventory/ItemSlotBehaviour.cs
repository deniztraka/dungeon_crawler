using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DTWorldz.Behaviours.Items;
using DTWorldz.DataModel;
using DTWorldz.Models;
using DTWorldz.SaveSystem;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Behaviours.UI.Inventory
{
    public class ItemSlotBehaviour : MonoBehaviour, IPointerClickHandler
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

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 2 || (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 2))
            {
                var itemModel = GetItem();
                if (itemModel != null)
                {

                    var inventoryBehaviour = transform.GetComponentInParent<InventoryBehaviour>();
                    if (inventoryBehaviour != null)
                    {
                        inventoryBehaviour.ShowItem(itemModel);
                    }
                }
            }
        }



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
            var child = transform.GetChild(0);
            if (child != null)
            {
                var slotItem = child.GetComponent<SlotItemBehaviour>();
                var playerObj = GameObject.FindWithTag("Player"); // playerın oldugu yere instantiate etçen

                StartCoroutine(LateDrop(slotItem, playerObj.transform.position, 1, 0.25f));
            }
        }

        IEnumerator LateDrop(SlotItemBehaviour slotItemBehaviour, Vector3 position, int count, float delay)
        {
            yield return new WaitForSeconds(delay);

            // changing item's position close to player randomly
            var randomPosition = new Vector3(position.x + UnityEngine.Random.Range(-0.5f, 0.5f), position.y + UnityEngine.Random.Range(-0.5f, 0.5f), position.z);
            slotItemBehaviour.ItemBehaviour.transform.position = randomPosition;
            // activate it again to show in world
            slotItemBehaviour.ItemBehaviour.gameObject.SetActive(true);

            DestroyImmediate(slotItemBehaviour.gameObject);
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

        internal void Save()
        {
            var saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            var persistentPath = Application.persistentDataPath;
            string filePath = Path.Combine(persistentPath, saveSystemManager.SavePath);
            //remove inventory file for this slot first           
            DirectoryInfo taskDirectory = new DirectoryInfo(filePath);
            var files = taskDirectory.GetFiles(gameObject.name + "_*.dnz");
            foreach (var file in files)
            {
                file.Delete();
            }

            if (transform.childCount > 0)
            {

                var itemType = GetItemType();
                var itemSlotDataModel = new ItemSlotDataModel(saveSystemManager, gameObject.name + '_' + itemType.ToString().ToLower());
                switch (itemType)
                {
                    case ItemType.Equipment:
                        //itemSlotDataModel.Item = GetItem() as EquipmentItemModel;
                        itemSlotDataModel.Save();
                        break;
                    case ItemType.Shield:

                        break;
                    case ItemType.Weapon:
                        //itemSlotDataModel.Item = GetItem() as WeaponItemModel;
                        itemSlotDataModel.OnSave<WeaponItemModel>(GetItem() as WeaponItemModel);
                        break;
                }

            }
        }

        internal void Load()
        {
            var saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            var itemSlotDataModel = new ItemSlotDataModel(saveSystemManager, gameObject.name);

            // check if weapontype exists
            var persistentPath = Application.persistentDataPath;
            string weaponfilePath = Path.Combine(persistentPath, "SaveData");
            weaponfilePath = Path.Combine(weaponfilePath, gameObject.name + "_weapon" + ".dnz");
            if (File.Exists(weaponfilePath))
            {
                itemSlotDataModel.DataObjName = gameObject.name + "_weapon";
                // saved weapon item
                var weaponItemModel = itemSlotDataModel.OnLoad<WeaponItemModel>();

                // instantiate slot item
                var slotItem = Instantiate(SlotItemPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<SlotItemBehaviour>();

                // instantiate weapon item
                var instantiatedWeapon = Instantiate(weaponItemModel.ItemTemplate.Prefab, Vector3.zero, Quaternion.identity).GetComponent<WeaponBehaviour>();
                instantiatedWeapon.Map(weaponItemModel);
                //wepon özelliklerini set et
                slotItem.SetItem(instantiatedWeapon);
                instantiatedWeapon.gameObject.SetActive(false);

            }

            // check if equipment type exists
            string equipmentfilePath = Path.Combine(persistentPath, "SaveData");
            equipmentfilePath = Path.Combine(equipmentfilePath, gameObject.name + "_equipment" + ".dnz");
            if (File.Exists(equipmentfilePath))
            {
                itemSlotDataModel.DataObjName = gameObject.name + "_equipment";
                var equipmentItemModel = itemSlotDataModel.OnLoad<EquipmentItemModel>();
            }

            // // check if shield type exists
            // string shieldfilePath = Path.Combine(persistentPath, "SaveData");
            // shieldfilePath = Path.Combine(shieldfilePath, gameObject.name + "_shield" + ".dnz");            
            // if (File.Exists(shieldfilePath))
            // {
            //     itemSlotDataModel.DataObjName = gameObject.name + "_shield";
            //     var weaponItemModel = itemSlotDataModel.OnLoad<ShieldItemModel>();
            // }

            // // check if None type exists
            // string nonefilePath = Path.Combine(persistentPath, "SaveData");
            // nonefilePath = Path.Combine(nonefilePath, gameObject.name + "_none" + ".dnz");            
            // if (File.Exists(nonefilePath))
            // {
            //     itemSlotDataModel.DataObjName = gameObject.name + "_none";
            //     var weaponItemModel = itemSlotDataModel.OnLoad<ShieldItemModel>();
            // }

            var ddSlot = GetComponent<DragAndDropSlot>();
            ddSlot.UpdateMyItem();
            ddSlot.UpdateBackgroundState();

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
        internal ItemType GetItemType()
        {
            var child = transform.GetChild(0);
            var slotItem = child.GetComponent<SlotItemBehaviour>();
            return slotItem.ItemBehaviour.ItemTemplate.ItemType;
        }
        internal ItemModel GetItem()
        {
            if (transform.childCount == 0)
            {
                return null;
            }

            var child = transform.GetChild(0);
            var slotItem = child.GetComponent<SlotItemBehaviour>();

            switch (GetItemType())
            {
                case ItemType.Weapon:
                    var weaponBehaviour = slotItem.ItemBehaviour.GetComponent<WeaponBehaviour>();
                    return weaponBehaviour.GetModel();
                case ItemType.Shield:

                    return null;
                case ItemType.Equipment:
                    var equipmentBehaviour = slotItem.ItemBehaviour.GetComponent<EquipmentBehaviour>();
                    return equipmentBehaviour.GetModel();
                default:
                    var itemModel = new ItemModel();
                    itemModel.ItemTemplate = slotItem.ItemBehaviour.ItemTemplate;
                    itemModel.StrengthModifier = slotItem.ItemBehaviour.StrengthModifier;
                    itemModel.DexterityModifier = slotItem.ItemBehaviour.DexterityModifier;
                    itemModel.StatQuality = slotItem.ItemBehaviour.StatQuality;
                    return itemModel;
            }

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