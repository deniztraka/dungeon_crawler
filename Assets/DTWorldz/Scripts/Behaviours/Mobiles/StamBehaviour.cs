using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Behaviours.Mobiles
{
    public class StamBehaviour : MonoBehaviour, IHealth
    {
        [SerializeField]
        private float currentVal;

        private float maxVal = 100;

        public AttackBehaviour AttackBehaviour;

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

            if (AttackBehaviour != null)
            {
                AttackBehaviour.OnAfterAttack += new AttackBehaviour.AttackingHandler(OnAfterAttack);
                AttackBehaviour.OnBeforeAttack += new AttackBehaviour.AttackingHandler(OnBeforeAttack);
            }

            if (OnHealthChanged != null)
            {
                OnHealthChanged(MaxHealth, MaxHealth);
            }
        }

        bool OnAfterAttack()
        {
            TakeDamage(AttackBehaviour.ActionPoint);
            return true;
        }

        bool OnBeforeAttack()
        {
            return currentVal >= AttackBehaviour.ActionPoint;
        }

        private void TakeDamage(float damage)
        {
            if (currentVal == -1)
            {
                return;
            }

            currentVal -= damage;

            if (OnDamageTaken != null)
            {
                OnDamageTaken(damage);
            }

            if (OnHealthChanged != null)
            {
                OnHealthChanged(currentVal, MaxHealth);
            }

            if (currentVal <= 0)
            {
                currentVal = -1;
            }
        }
    }
}
