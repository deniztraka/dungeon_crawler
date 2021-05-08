using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using DTWorldz.Items.SO;
using UnityEngine;
namespace DTWorldz.Behaviours.Mobiles
{
    public class HungerBehaviour : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float currentVal;

        private float maxVal = 100;

        public float IncreaseAmountEverySecond = 1;

        public float CurrentHealth
        {
            get { return currentVal; }
            set { currentVal = value; }
        }

        public float MaxHealth
        {
            get { return maxVal; }
            set { maxVal = value; }
        }

        public event HealthChanged OnHealthChanged;

        public delegate void DamageTaken(float damageAmount);
        public event DamageTaken OnDamageTaken;



        void Start()
        {
            currentVal = MaxHealth;

            if (OnHealthChanged != null)
            {
                OnHealthChanged(MaxHealth, MaxHealth);
            }

            InvokeRepeating("IncreaseOverTime", 1f, 1f);
        }

        void IncreaseOverTime()
        {
            currentVal += IncreaseAmountEverySecond;
            if (currentVal < 0)
            {
                currentVal = 0;
                return;
            }
            if (currentVal >= MaxHealth)
            {
                currentVal = MaxHealth;
            }
            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentVal, MaxHealth);
            }
        }

        public void Eat(FoodItemSO foodItem)
        {
            currentVal += !foodItem.IsCooked ? foodItem.RegenAmount : foodItem.RegenAmount * 1.25f;
            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentVal, MaxHealth);
            }
        }
    }
}