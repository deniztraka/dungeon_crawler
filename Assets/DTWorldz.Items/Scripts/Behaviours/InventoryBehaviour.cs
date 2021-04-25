using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Items.Behaviours
{
    public class InventoryBehaviour : MonoBehaviour
    {
        public ItemContainerSO ItemContainer;
        public int MaxItemCount = 30;

        AudioManager audioManager;

        public void AddItem(ItemBehaviour item)
        {
            if (ItemContainer != null && ItemContainer.ItemSlots.Count < MaxItemCount)
            {
                ItemContainer.AddItem(item.ItemSO);
                Destroy(item.gameObject);
                if (audioManager != null)
                {
                    audioManager.Play("Loot");
                }
            }

        }

        void Start()
        {
            audioManager = gameObject.GetComponent<AudioManager>();
        }

    }
}