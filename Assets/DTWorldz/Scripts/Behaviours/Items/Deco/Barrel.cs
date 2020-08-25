using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Deco
{
    public class Barrel : MonoBehaviour
    {
        private HealthBehaviour healthBehaviour;

        void Start()
        {
            healthBehaviour = GetComponent<HealthBehaviour>();
            if (healthBehaviour != null)
            {
                healthBehaviour.OnHealthChanged += new HealthBehaviour.HealthChanged(HealthChanged);
                healthBehaviour.OnDeath += new HealthBehaviour.HealthChanged(OnDeath);                
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void HealthChanged(float currentHealth, float maxHealth)
        {
            //Debug.Log("health is changed");
        }

        private void OnDeath(float currentHealth, float maxHealth)
        {
            //Debug.Log("dead");
            Destroy(gameObject, 1);
        }
    }
}
