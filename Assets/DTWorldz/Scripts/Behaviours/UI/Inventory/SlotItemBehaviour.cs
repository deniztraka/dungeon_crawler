using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Items;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI.Inventory
{
    public class SlotItemBehaviour : MonoBehaviour
    {
        public GameObject ItemQuantityPanel;
        public GameObject ItemTexturePanel;

        public BaseItemBehaviour ItemBehaviour;

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

            ItemBehaviour = itemBehaviour;            
            // this.ItemTemplate = itemBehaviour.ItemTemplate;

            // StrengthModifier = itemBehaviour.StrengthModifier;
            // DexterityModifier = itemBehaviour.DexterityModifier;
            // StatQuality = itemBehaviour.StatQuality;
            // MinDamage = itemBehaviour.MinDamage;
            // MaxDamage = itemBehaviour.MaxDamage;
        }
    }
}