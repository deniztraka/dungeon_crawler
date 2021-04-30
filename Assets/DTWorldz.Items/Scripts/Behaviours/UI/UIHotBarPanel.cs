using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.Behaviours.UI
{
    public class UIHotBarPanel : MonoBehaviour
    {
        [SerializeField]
        private InventoryUI InventoryUI;

        private List<HotBarSlotBehaviour> SlotBehaviours;
        // Start is called before the first frame update
        void Start()
        {
            InventoryUI.OnInventoryOpened += new InventoryUI.InventoryUIHandler(InventoryOpened);
            InventoryUI.OnInventoryClosed += new InventoryUI.InventoryUIHandler(InventoryClosed);
            InventoryUI.OnInventoryRefreshed += new InventoryUI.InventoryUIHandler(InventoryRefreshed);
            SlotBehaviours = new List<HotBarSlotBehaviour>();
            SlotBehaviours.AddRange(GetComponentsInChildren<HotBarSlotBehaviour>());
        }

        private void InventoryRefreshed()
        {
            foreach (var slot in SlotBehaviours)
            {
                slot.Refresh();
            }
        }

        private void InventoryClosed()
        {
            foreach (var slot in SlotBehaviours)
            {
                slot.ToggleRemoveButton(false);
                slot.ToggleUseButton(true);
            }
        }

        private void InventoryOpened()
        {
            foreach (var slot in SlotBehaviours)
            {
                slot.ToggleRemoveButton(true);
                slot.ToggleUseButton(false);
            }
        }

    }
}
