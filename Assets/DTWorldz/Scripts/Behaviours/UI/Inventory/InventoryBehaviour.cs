using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.UI.Inventory;
using UnityEngine;

public class InventoryBehaviour : MonoBehaviour
{
    public static InventoryBehaviour Instance { get; private set; }

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

    internal bool AddItem(BaseItemBehaviour itemBehaviour)
    {
        //check empty slot for this item
        var emptySlot = FindEmptySlot();
        if (emptySlot == null)
        {
            return false;
        }

        emptySlot.AddItem(itemBehaviour);
        return true;
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
}
