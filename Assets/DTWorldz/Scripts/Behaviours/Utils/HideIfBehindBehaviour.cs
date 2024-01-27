using System.Collections;
using System.Collections.Generic;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class HideIfBehindBehaviour : MonoBehaviour
    {
        public float OnOverOpacity = 0.75f;
        public SpriteRenderer RendererToHide;
        private GameObject player;
        private Color initialColor;
        // Start is called before the first frame update
        void Start()
        {
            player = GameManager.Instance.PlayerBehaviour.gameObject;
            if (RendererToHide)
            {
                initialColor = RendererToHide.color;
            }
        }

        public void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                if (RendererToHide)
                {
                    var newColor = new Color(initialColor.r, initialColor.g, initialColor.b, OnOverOpacity);
                    RendererToHide.color = newColor;
                }
            }
        }

        public void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "Player")
            {
                if (RendererToHide)
                {
                    RendererToHide.color = initialColor;
                }
            }
        }
    }
}