using System;
using System.Collections;
using System.Collections.Generic;
using DTWorlds.Behaviours.Effects;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.UI;
using DTWorldz.Interfaces;
using UnityEngine;

namespace DTWorldz.Behaviours.Mobiles
{
    public class HealthBehaviour : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float currentHealth;

        [SerializeField]
        private float maxHealth = 100;
        public BodyType BodyType;
        public Color OnDamageTakenTintColor;

        public float CurrentHealth
        {
            get { return currentHealth; }
            set
            {
                if (OnHealthChanged != null && currentHealth != value)
                {
                    OnHealthChanged(value, MaxHealth);
                }
                currentHealth = value;
            }
        }

        public float MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }



        public float IncreaseAmountEverySecond = 1;

        public delegate void DamageTaken(float damageAmount, DamageType type);
        public event DamageTaken OnDamageTaken;
        public event HealthChanged OnHealthChanged;
        public event HealthChanged OnBeforeHealthChanged;
        public event HealthChanged OnDeath;
        public GameObject FloatingDamagesPrefab;
        public float DamagePointsYOffset;
        public bool MaxOnStart = true;
        private AudioManager audioManager;
        private MaterialTintColor materialTintColor;

        private Harvestable harvestable;

        void Start()
        {
            materialTintColor = GetComponent<MaterialTintColor>();
            if (MaxOnStart)
            {
                currentHealth = MaxHealth;
            }

            audioManager = GetComponent<AudioManager>();
            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentHealth, MaxHealth);
            }

            InvokeRepeating("IncreaseOverTime", 1f, 1f);

            harvestable = GetComponentInChildren<Harvestable>();
        }

        void IncreaseOverTime()
        {
            if (currentHealth >= 0)
            {
                currentHealth += IncreaseAmountEverySecond;
                if (currentHealth >= MaxHealth)
                {
                    currentHealth = MaxHealth;
                }
                if (OnHealthChanged != null)
                {
                    OnHealthChanged(currentHealth, MaxHealth);
                }
            }
        }

        public void TakeDamage(float damage, DamageType type)
        {
            if (currentHealth == -1)
            {
                return;
            }

            if (OnBeforeHealthChanged != null)
            {
                OnBeforeHealthChanged(currentHealth, MaxHealth);
            }

            if (harvestable != null && harvestable.HasHarvest())
            {
                harvestable.Harvest();
                return;
            }

            currentHealth -= damage;
            if (materialTintColor != null)
            {
                materialTintColor.SetTintColor(OnDamageTakenTintColor);
            }

            if (FloatingDamagesPrefab != null)
            {
                PopUpFloatingDamages(damage);
            }

            if (audioManager != null && UnityEngine.Random.value > 0.5f)
            {
                audioManager.Play("Hit");
            }

            if (OnDamageTaken != null)
            {
                OnDamageTaken(currentHealth, DamageType.Physical);
            }

            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentHealth, MaxHealth);
            }

            if (currentHealth <= 0)
            {
                currentHealth = -1;
                if (OnDeath != null)
                {
                    if (audioManager != null)
                    {
                        audioManager.Play("Dead");
                    }
                    OnDeath(currentHealth, MaxHealth);
                }
            }
        }

        private void PopUpFloatingDamages(float damage)
        {
            var randomXOffSet = UnityEngine.Random.Range(-0.1f, 0.2f);
            var newPos = new Vector3(transform.position.x + randomXOffSet, transform.position.y + DamagePointsYOffset, transform.position.z);

            var floatingDamage = Instantiate(FloatingDamagesPrefab, newPos, Quaternion.identity, transform);
            var floatingText = floatingDamage.GetComponent<FloatingTextBehaviour>();
            floatingText.SetText(String.Format("{0:0}", damage));
        }
    }
}