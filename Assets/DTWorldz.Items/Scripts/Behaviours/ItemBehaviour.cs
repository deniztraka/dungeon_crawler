using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using DTWorldz.Items.SO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Items.Behaviours
{
    public class ItemBehaviour : MonoBehaviour
    {
        public BaseItemSO ItemSO;
        public int Quantity;
        [SerializeField]
        private Sprite unsetSprite;
        private SpriteRenderer spriteRenderer;
        private Vector3 tempPosition;
        private bool dragging;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            dragging = false;
        }

        void OnValidate()
        {
            SetSprite();
        }

        private void SetSprite()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (ItemSO != null)
            {
                spriteRenderer.sprite = ItemSO.Icon;
            }
            else
            {
                spriteRenderer.sprite = unsetSprite;
            }
        }
    }
}