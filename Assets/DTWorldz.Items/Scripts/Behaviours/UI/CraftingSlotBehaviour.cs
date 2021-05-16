using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Items.Behaviours.UI
{
    public class CraftingSlotBehaviour : ItemSlotBehaviour
    {
        public override void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DragEndDropCraftingPanel", this);
        }
    }
}