using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Behaviours
{
    public class Harvestable : MonoBehaviour
    {
        [SerializeField]
        private int count;
        public GameObject HarvestItemPrefab;

        void Start()
        {
            // health = GetComponent<HealthBehaviour>();
            // if (health == null)
            // {
            //     health = GetComponentInParent<HealthBehaviour>();
            //     health.OnBeforeHealthChanged += new Interfaces.HealthChanged(Harvest);
            // }

        }

        public void Harvest()
        {
            if (HarvestItemPrefab != null && HasHarvest())
            {
                for (int i = 0; i < count; i++)
                {
                    
                    var randomPosition = new Vector3(transform.position.x + UnityEngine.Random.Range(-1, 1), transform.position.y + UnityEngine.Random.Range(-1, 1), transform.position.z);
                    Instantiate(HarvestItemPrefab, randomPosition, Quaternion.identity);
                    count--;
                }
                Destroy(gameObject, 0.5f);
            }
        }

        public bool HasHarvest()
        {
            return HarvestItemPrefab != null && count > 0;
        }



    }
}