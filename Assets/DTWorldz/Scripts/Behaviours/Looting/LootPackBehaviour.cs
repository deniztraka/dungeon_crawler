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
            healthBehaviour.OnDeath += new HealthChanged(DropLoot);
        }



        IEnumerator LateDrop(GameObject prefab, Vector3 position, int count, float delay)
        {
            yield return new WaitForSeconds(delay);
            var instantiatedLootItem = Instantiate(prefab, position, Quaternion.identity);
            var lootItem = instantiatedLootItem.GetComponent(typeof(ILootItem)) as ILootItem;            
            lootItem.SetCount(count);
            lootItem.OnAfterDrop();
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
                    StartCoroutine(LateDrop(itemPrefab, transform.position, itemCount, 0.5f));
                }
            }
        }
    }
}