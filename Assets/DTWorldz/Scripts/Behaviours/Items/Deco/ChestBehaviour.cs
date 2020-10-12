using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Looting;
using DTWorldz.Behaviours.Utils;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Deco
{
    public class ChestBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool isOpen;
        private Interactable interactable;
        private LootPackBehaviour lootPack;
        private SpriteRenderer spriteRenderer;
        private AudioManager audioManager;
        public Sprite OpenSprite;
        void Start()
        {
            interactable = GetComponent<Interactable>();
            lootPack = GetComponent<LootPackBehaviour>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            audioManager = GetComponent<AudioManager>();
            if (interactable != null)
            {
                interactable.OnInteraction += new Interactable.InteractHandler(OnInteraction);
            }
        }

        private void OnInteraction()
        {
            TryOpen();
        }

        public virtual bool TryOpen()
        {
            if (!isOpen)
            {
                isOpen = true;
                if (lootPack)
                {
                    lootPack.DropLoot();
                }
                spriteRenderer.sprite = OpenSprite;
                if (audioManager)
                {
                    audioManager.Play("Open");
                }
                return true;
            }
            return false;
        }
    }
}