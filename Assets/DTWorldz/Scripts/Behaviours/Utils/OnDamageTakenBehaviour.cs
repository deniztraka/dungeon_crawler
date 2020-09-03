using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    [RequireComponent(typeof(HealthBehaviour))]
    public class OnDamageTakenBehaviour : MonoBehaviour
    {
        public ParticleSystem DamageTakenEffectPrefab;
        private HealthBehaviour healthBehaviour;
        private ParticleSystem damageTakenEffect;


        void Start()
        {
            healthBehaviour = gameObject.GetComponent<HealthBehaviour>();
            if (healthBehaviour != null)
            {
                healthBehaviour.OnDamageTaken += new HealthBehaviour.DamageTaken(OnDamageTaken);
            }

            if (DamageTakenEffectPrefab != null)
            {
                var damageTakenEffectObj = Instantiate(DamageTakenEffectPrefab, transform.position, Quaternion.identity, this.transform);
                damageTakenEffect = damageTakenEffectObj.GetComponent<ParticleSystem>();
            }
        }

        protected virtual void OnDamageTaken(float damageAmount, DamageType type)
        {
            if (damageTakenEffect != null && !damageTakenEffect.isPlaying)
            {
                damageTakenEffect.Play();
            }
            Debug.Log(gameObject.name + " is taken " + damageAmount + " " + type.ToString() + " damage.");
        }
    }
}