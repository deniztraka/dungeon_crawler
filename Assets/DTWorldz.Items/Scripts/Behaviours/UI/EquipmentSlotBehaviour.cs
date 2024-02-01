using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public enum EquipmentType
    {
        Head,
        Chest,
        Legs,
        Feet,
        LeftHand,
        RightHand,
    }

    public class EquipmentSlotBehavior : ItemSlotBehaviour
    {

        public delegate void ItemEquippedHandler(BaseItemSO itemSO);
        public event ItemEquippedHandler OnItemEquipped;
        public event ItemEquippedHandler OnItemUnequipped;

        private InventoryBehaviour inventoryBehaviour;
        private BaseItemSO equippedItemSO;
        public EquipmentType EquipmentType;

        public override void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DropToEquipmentSlot", this);
        }

        public BaseItemSO Switch(BaseItemSO itemSO)
        {
            if (itemSO == null)
            {
                return null;
            }

            if (equippedItemSO != null && equippedItemSO.Id == itemSO.Id)
            {
                return null;
            }

            if (itemSO is not WeaponItemSO)
            {
                return null;
            }

            var oldItemSO = equippedItemSO;
            SetItem(itemSO, 1);
            return oldItemSO;
        }

        internal override void SetItem(BaseItemSO itemSO, int quantity)
        {
            //Debug.Log("SetItemEquipmentSlot");

            if(itemSO is not WeaponItemSO){
                return;
            }

            var icon = GetIcon();
            if (itemSO == null)
            {
                if (icon != null)
                {
                    icon.sprite = null;
                    icon.color = new Color(255, 255, 255, 0);
                }
                return;
            }


            if (icon != null)
            {
                icon.enabled = true;
                icon.sprite = itemSO.Icon;
                icon.color = new Color(255, 255, 255, 1);
            }

            

            // there is item switch here
            if(equippedItemSO != null && equippedItemSO.Id != itemSO.Id){
                // fire unequip event
                //Debug.Log("OnItemUneqquiped: " + equippedItemSO.Name);
                if (OnItemEquipped != null)
                {
                    OnItemUnequipped.Invoke(equippedItemSO);
                }
            }

            equippedItemSO = itemSO;
            // fire equip event
            //Debug.Log("OnItemEqquiped: " + itemSO.Name);
            if (OnItemEquipped != null)
            {
                OnItemEquipped.Invoke(itemSO);
            }
        }

        internal override BaseItemSO GetItem()
        {
            return equippedItemSO;
        }

        public override int GetQuantity()
        {
            return 1;
        }

        internal override void RemoveItem()
        {
            var icon = GetIcon();

            if (icon != null)
            {
                icon.color = new Color(255, 255, 255, 0f);
                icon.sprite = null;
                icon.enabled = false;

                //Debug.Log("RemoveItemEquipmentSlot");
            }

            // fire unequip event
            //Debug.Log("OnItemUneqquiped: " + equippedItemSO.Name);
            if (OnItemEquipped != null)
            {
                OnItemUnequipped.Invoke(equippedItemSO);
            }

            equippedItemSO = null;
        }
    }
}