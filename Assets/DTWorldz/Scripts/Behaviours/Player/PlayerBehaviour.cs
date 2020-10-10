using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.UI;
using UnityEngine;
namespace DTWorldz.Behaviours.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        public ActionButtonBehaviour ActionButtonBehaviour;
        public ActionButtonBehaviour HealthPotionButtonBehaviour;
        public ActionButtonBehaviour StaminaPotionButtonBehaviour;
        // Start is called before the first frame update
        void Start()
        {
            var movementBehaviour = GetComponent<PlayerMovementBehaviour>();
            var attackBehaviour = GetComponentInChildren<AttackBehaviour>();
            ActionButtonBehaviour.SetAction(movementBehaviour.Attack, attackBehaviour.AttackingFrequency);                  
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
            Debug.Log(count + " gold piece" + (isPlural ? "s " : " ")  + (isPlural ? "are" : "is") + " collected.");
        }
    }
}
