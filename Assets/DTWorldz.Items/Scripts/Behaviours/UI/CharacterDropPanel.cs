using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace DTWorldz.Items.Behaviours.UI
{
    public class CharacterDropPanel : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            SendMessageUpwards("DropToCharacterBar");
        }
    }
}