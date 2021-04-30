using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Items.Behaviours.UI
{
    public class UIDropPanel : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DragEndDropItem");
        }
    }
}