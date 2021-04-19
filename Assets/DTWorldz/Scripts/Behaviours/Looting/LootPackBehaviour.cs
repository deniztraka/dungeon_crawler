using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
using DTWorldz.Models;
using DTWorldz.ScriptableObjects;
using UnityEngine;

namespace DTWorldz.Behaviours.Looting
{
    public class LootPackBehaviour : MonoBehaviour
    {
        public float SpreadDistance = 0.5f;
        public float DropAfterSeconds = 0.5f;
        public ItemDropTemplate DropTemplate;
        private HealthBehaviour healthBehaviour;
        // Start is called before the first frame update
        void Start()
        {
            healthBehaviour = gameObject.GetComponent<HealthBehaviour>();
            if (healthBehaviour)
            {
                healthBehaviour.OnDeath += new HealthChanged(DropLoot);
            }
        }

        IEnumerator LateDrop(LootPackItem lootPackItem, Vector3 position, int count, float delay)
        {
            yield return new WaitForSeconds(delay);
            var randomPosition = new Vector3(position.x + UnityEngine.Random.Range(-SpreadDistance, SpreadDistance), position.y + UnityEngine.Random.Range(-SpreadDistance, SpreadDistance), position.z);
            var instantiatedLootItem = Instantiate(lootPackItem.ItemPrefab, randomPosition, Quaternion.identity);
            var lootItem = instantiatedLootItem.GetComponent(typeof(ILootItem)) as ILootItem;
            lootItem.SetCount(count);
            lootItem.SetModifiers(lootPackItem.MinStatCount, lootPackItem.MaxStatCount, lootPackItem.StatQuality);
            lootItem.OnAfterDrop();
        }

        internal void DropLoot()
        {
            DropLoot(0, 0);
        }

        private void DropLoot(float killedMobHealth, float killedMobMaxHealth)
        {
            foreach (var lootEntry in DropTemplate.Entries)
            {
                var dropCount = UnityEngine.Random.Range(lootEntry.MinCount, lootEntry.MaxCount);


                for (int i = 0; i < dropCount; i++)
                {
                    var dropChance = UnityEngine.Random.value;
                    if (dropChance < lootEntry.Chance)
                    {

                        var lootPackItem = lootEntry.Items[UnityEngine.Random.Range(0, lootEntry.Items.Count)];
                        var itemCount = UnityEngine.Random.Range(lootPackItem.CountMin, lootPackItem.CountMax);
                        var itemPrefab = lootPackItem.ItemPrefab;
                        StartCoroutine(LateDrop(lootPackItem, transform.position, itemCount, DropAfterSeconds));
                    }
                }

            }
        }
    }
}