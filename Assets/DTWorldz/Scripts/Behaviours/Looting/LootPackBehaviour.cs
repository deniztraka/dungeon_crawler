﻿using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
using DTWorldz.ScriptableObjects;
using UnityEngine;

namespace DTWorldz.Behaviours.Looting
{
    public class LootPackBehaviour : MonoBehaviour
    {
        public ItemDropTemplate DropTemplate;
        private HealthBehaviour healthBehaviour;
        // Start is called before the first frame update
        void Start()
        {
            healthBehaviour = gameObject.GetComponent<HealthBehaviour>();
            healthBehaviour.OnDeath += new HealthChanged(DropLoot);
        }



        IEnumerator LateDrop(GameObject prefab, Vector3 position, int count, float delay)
        {
            yield return new WaitForSeconds(delay);
            var randomPosition = new Vector3(position.x + UnityEngine.Random.Range(-0.5f, 0.5f), position.y + UnityEngine.Random.Range(-0.5f, 0.5f), position.z);
            var instantiatedLootItem = Instantiate(prefab, randomPosition, Quaternion.identity);
            var lootItem = instantiatedLootItem.GetComponent(typeof(ILootItem)) as ILootItem;
            lootItem.SetCount(count);
            lootItem.OnAfterDrop();
        }

        void DropLoot(float killedMobHealth, float killedMobMaxHealth)
        {
            foreach (var lootEntry in DropTemplate.Entries)
            {
                var dropCount = UnityEngine.Random.Range(1, lootEntry.MaxCount);
                
                
                for (int i = 0; i < dropCount; i++)
                {
                    var dropChance = UnityEngine.Random.value;
                    if (dropChance < lootEntry.Chance)
                    {
                        var lootPackItem = lootEntry.Items[UnityEngine.Random.Range(0, lootEntry.Items.Count)];
                        var itemCount = UnityEngine.Random.Range(lootPackItem.CountMin, lootPackItem.CountMax);
                        var itemPrefab = lootPackItem.ItemPrefab;
                        StartCoroutine(LateDrop(itemPrefab, transform.position, itemCount, 0.5f));
                    }
                }

            }
        }
    }
}