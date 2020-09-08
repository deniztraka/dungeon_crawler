using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Player;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Utils
{
    public class GoldItemBehaviour : BaseItemBehaviour, ILootItem
    {
        [SerializeField]
        private int count = 1;
        public Sprite SinglePieceSprite;
        public Sprite TwoPiecesSprite;
        public Sprite FourPiecesSprite;
        public Sprite ManyPiecesSprite;
        public Sprite RichPiecesSprite;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private AudioManager audioManager;

        public int Count
        {
            get { return count; }
        }

        public bool isStackable = true;
        public bool IsStackable
        {
            get
            {
                return isStackable;
            }
            set
            {
                isStackable = value;
            }
        }
        public void SetCount(int count)
        {
            if (isStackable)
            {
                this.count += count;
            }
            else
            {
                throw new NotImplementedException("Could not set count on unstackable item behaviour ");
            }

            UpdateSprite();
        }

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
            audioManager = GetComponent<AudioManager>();
            UpdateSprite();
            if (audioManager != null)
            {
                audioManager.Play("Drop");
            }
        }

        public void OnAfterDrop()
        {
            if (audioManager != null)
            {
                audioManager.Play("Drop");
            }
            UpdateSprite();
        }

        public void UpdateSprite()
        {
            if (spriteRenderer != null)
            {
                if (count < 2)
                {
                    spriteRenderer.sprite = SinglePieceSprite;
                    return;
                }
                else if (count < 4)
                {
                    spriteRenderer.sprite = TwoPiecesSprite;
                    return;
                }
                else if (count < 5)
                {
                    spriteRenderer.sprite = FourPiecesSprite;
                    return;
                }
                else if (count < 100)
                {
                    spriteRenderer.sprite = ManyPiecesSprite;
                    return;
                }
                else
                {
                    spriteRenderer.sprite = RichPiecesSprite;
                    return;
                }
            }
        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.tag == "Player")
            {
                var playerBehaviour = collider.gameObject.GetComponent<PlayerBehaviour>();
                playerBehaviour.CollectGold(this.count);

                var spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
                var tempColor = spriteRenderer.color;
                tempColor.a = 0f;
                spriteRenderer.color = tempColor;    
                var thisCollider = gameObject.GetComponentInChildren<Collider2D>();            
                thisCollider.enabled = false;

                if (audioManager != null)
                {
                    audioManager.Play("Pickup");
                }
                Destroy(this.gameObject, 1);
            }
        }
    }
}