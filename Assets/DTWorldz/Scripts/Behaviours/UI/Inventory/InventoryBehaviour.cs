using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.UI.Inventory;
using DTWorldz.DataModel;
using DTWorldz.SaveSystem;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;

public class InventoryBehaviour : MonoBehaviour
{
    private InventoryDataModel inventoryDataModel;
    private SaveSystemManager saveSystemManager;
    public delegate void DataLoaderHandler();
    public event DataLoaderHandler OnAfterDataLoad;

    public static InventoryBehaviour Instance { get; private set; }

    internal void DropItem(ItemSlotBehaviour itemSlotBehaviour)
    {
        itemSlotBehaviour.DropItem();
        RefreshItemsData();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RegisterToSaveSystem();
    }

    public void RegisterToSaveSystem()
    {
        saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
        if (saveSystemManager)
        {
            OnAfterDataLoad += new DataLoaderHandler(UpdateUI);
            Load();
            saveSystemManager.OnGameSave += new SaveSystemManager.SaveSystemHandler(Save);
        }
    }

    internal bool AddItem(BaseItemBehaviour itemBehaviour)
    {
        //check empty slot for this item
        var emptySlot = FindEmptySlot();
        if (emptySlot == null)
        {
            return false;
        }

        emptySlot.AddItem(itemBehaviour);

        RefreshItemsData();
        return true;
    }

    public void RefreshItemsData()
    {
        if (inventoryDataModel != null)
        {
            inventoryDataModel.Items = GetItems();
            Debug.Log(inventoryDataModel.Items.Count);
        }
    }

    private ItemSlotBehaviour FindEmptySlot()
    {
        var itemSlots = transform.GetComponentsInChildren<ItemSlotBehaviour>();
        for (int i = 0; i < itemSlots.Length; i++)
        {
            var slotBehaviour = itemSlots[i];
            if (slotBehaviour.transform.childCount == 0)
            {
                return slotBehaviour;
            }

        }

        return null;
    }

    private void UpdateUI()
    {
        foreach (var item in inventoryDataModel.Items)
        {
            var itemBehaviour = item.Prefab.GetComponent<BaseItemBehaviour>();
            AddItem(itemBehaviour);
        }
    }

    public bool Load()
    {
        if (saveSystemManager != null && saveSystemManager.HasSavedGame())
        {
            inventoryDataModel = new InventoryDataModel(saveSystemManager);
            inventoryDataModel.Load();

            if (OnAfterDataLoad != null)
            {
                OnAfterDataLoad();
            }

            return true;
        }
        return false;
    }

    private List<BaseItem> GetItems()
    {
        var itemsInside = new List<BaseItem>();
        var itemSlots = transform.GetComponentsInChildren<ItemSlotBehaviour>();

        foreach (var itemSlot in itemSlots)
        {
            var itemInside = itemSlot.GetItem();
            if (itemInside != null)
            {
                itemsInside.Add(itemInside);
            }
        }
        return itemsInside;
    }

    public void Save()
    {
        if (saveSystemManager != null && inventoryDataModel != null)
        {
            inventoryDataModel.Save();
        }
    }
}
