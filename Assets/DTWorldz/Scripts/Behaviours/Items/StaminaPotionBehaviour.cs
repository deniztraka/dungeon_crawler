using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Player;
using DTWorldz.Interfaces;
using DTWorldz.Models.MobileStats;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Utils
{
    public class StaminaPotionBehaviour : BaseItemBehaviour, ILootItem
    {
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private AudioManager audioManager;

        // Start is called before the first frame update
        void Start()
        {
            spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
            audioManager = GetComponent<AudioManager>();
            isStackable = false;
            if (audioManager != null)
            {
                audioManager.Play("Drop");
            }            
        }

        public override void OnAfterDrop()
        {
            if (audioManager != null)
            {
                audioManager.Play("Drop");
            }
        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.tag == "Player")
            {
                var playerBehaviour = collider.gameObject.GetComponent<PlayerBehaviour>();
                playerBehaviour.CollectStaminaPotion();

                var spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
                var tempColor = spriteRenderer.color;
                tempColor.a = 0f;
                spriteRenderer.color = tempColor;
                var thisCollider = gameObject.GetComponentInChildren<Collider2D>();
                thisCollider.enabled = false;
                Destroy(this.gameObject, 1);
            }
        }

        public override void SetCount(int count)
        {
            this.count = count;
        }

        public override void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality)
        {
            // no modifiers for stamina potion
        }
    }
}