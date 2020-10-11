using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.UI;
using UnityEngine;
namespace DTWorldz.Behaviours.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        AudioManager audioManager;
        HealthBehaviour health;
        StamBehaviour stamina;
        public ActionButtonBehaviour ActionButtonBehaviour;
        public HealthPotionButtonBehaviour HealthPotionButtonBehaviour;
        public StaminaPotionButtonBehaviour StaminaPotionButtonBehaviour;
        // Start is called before the first frame update
        void Start()
        {
            var movementBehaviour = GetComponent<PlayerMovementBehaviour>();
            var attackBehaviour = GetComponentInChildren<AttackBehaviour>();
            ActionButtonBehaviour.SetAction(movementBehaviour.Attack, attackBehaviour.AttackingFrequency);
            health = GetComponent<HealthBehaviour>();
            stamina = GetComponent<StamBehaviour>();
            audioManager = gameObject.GetComponent<AudioManager>();
        }



        // Update is called once per frame
        void Update()
        {
            // if(Input.GetMouseButton(0)){
            //     var health = gameObject.GetComponent<HealthBehaviour>();
            //     if(health != null){
            //         health.TakeDamage(5, DamageType.Physical);
            //     }
            // }
        }

        internal void CollectGold(int count)
        {
            var isPlural = count > 1;
            Debug.Log(count + " gold piece" + (isPlural ? "s " : " ") + (isPlural ? "are" : "is") + " collected.");
        }

        internal void DrinkHealthPotion()
        {
            audioManager.Play("Drink");
            health.CurrentHealth += 20;
        }
        internal void DrinkStaminaPotion()
        {
            audioManager.Play("Drink");
            stamina.CurrentHealth += 30;
        }

        internal void CollectHealthPotion()
        {
            audioManager.Play("Loot");
            HealthPotionButtonBehaviour.AddPotion();
        }

        internal void CollectStaminaPotion()
        {
            audioManager.Play("Loot");
            StaminaPotionButtonBehaviour.AddPotion();
        }
    }
}
