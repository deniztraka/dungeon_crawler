using System.Collections;
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
            healthBehaviour.OnDeath += new HealthBehaviour.HealthChanged(DropLoot);
        }

        void DropLoot(float killedMobHealth, float killedMobMaxHealth)
        {
            foreach (var lootEntry in DropTemplate.Entries)
            {
                if (UnityEngine.Random.value < lootEntry.Chance)
                {
                    var lootPackItem = lootEntry.Items[UnityEngine.Random.Range(0, lootEntry.Items.Count)];
                    var itemCount = UnityEngine.Random.Range(lootPackItem.CountMin, lootPackItem.CountMax);
                    var itemPrefab = lootPackItem.ItemPrefab;

                    var instantiatedLootItem = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                    var lootItem = instantiatedLootItem.GetComponent(typeof(ILootItem)) as ILootItem;
                    lootItem.SetCount(itemCount);
                    lootItem.OnAfterDrop();
                }
            }
        }
    }
}