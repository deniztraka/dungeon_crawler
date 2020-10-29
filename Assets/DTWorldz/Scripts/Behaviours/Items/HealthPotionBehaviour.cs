﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Player;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Utils
{
    public class HealthPotionBehaviour : BaseItemBehaviour, ILootItem
    {
        [SerializeField]
        private int count;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private AudioManager audioManager;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        private bool isStackable;
        public bool IsStackable
        {
            get { return isStackable; }
            set { isStackable = value; }
        }


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

        public void OnAfterDrop()
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
                playerBehaviour.CollectHealthPotion();

                var spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
                var tempColor = spriteRenderer.color;
                tempColor.a = 0f;
                spriteRenderer.color = tempColor;
                var thisCollider = gameObject.GetComponentInChildren<Collider2D>();
                thisCollider.enabled = false;
                Destroy(this.gameObject, 1);
            }
        }

        public void SetCount(int count)
        {
            this.count = count;
        }
    }
}