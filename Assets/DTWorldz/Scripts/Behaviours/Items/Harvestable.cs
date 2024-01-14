using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DTWorldz.Behaviours
{
    public class Harvestable : MonoBehaviour
    {
        [SerializeField]
        private int count;
        public GameObject HarvestItemPrefab;

        private Interactable interactable;

        void Start()
        {
            interactable = GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.OnInteraction += new Interactable.InteractHandler(OnInteraction);
            }

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
                Destroy(gameObject,0.1f);
            }
        }

        public bool HasHarvest()
        {
            return HarvestItemPrefab != null && count > 0;
        }

        private void OnInteraction()
        {
            Harvest();
        }

    }
}