using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI.Inventory
{
    public class SlotItemBehaviour : MonoBehaviour
    {
        //public Item Item;

        public GameObject ItemQuantityPanel;
        public GameObject ItemTexturePanel;

        // internal void Stack(Item item)
        // {
        //    throw new NotImplementedException();
        // }

        internal void SetUI(Sprite icon)
        {
            throw new NotImplementedException();
        }

        internal bool UseItem()
        {
            throw new NotImplementedException();
        }

        internal void SetItemAmount(int newAmount)
        {
            throw new NotImplementedException();
        }

        internal void SetItem(BaseItemBehaviour itemBehaviour)
        {
            var rect = GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;

            var spriteRenderer = itemBehaviour.GetComponentInChildren<SpriteRenderer>();
            var image = GetComponent<Image>();
            image.sprite = spriteRenderer.sprite;
        }
    }
}