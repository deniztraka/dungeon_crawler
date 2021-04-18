using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Crafting.Resources
{
    public class LogBehaviour : BaseItemBehaviour, ILootItem
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

        public override void SetCount(int count)
        {
            this.count = count;
        }
    }
}