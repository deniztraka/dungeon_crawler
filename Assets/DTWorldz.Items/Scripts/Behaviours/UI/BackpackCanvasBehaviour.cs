using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTWorldz.Items.Behaviours.UI
{
    public class BackpackCanvasBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Sprite openSprite;
        private Sprite closedSprite;
        private Image Image;

        void Start()
        {
            Image = GetComponent<Image>();
            closedSprite = Image.sprite;
        }
        private bool isOpen = false;
        internal void SetStatus(bool isOpen)
        {
            Image.sprite = isOpen ? openSprite : closedSprite;
        }
    }
}