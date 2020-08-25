using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Deco
{
    public class TrapBehaviour : MonoBehaviour
    {
        public int RefreshTime;
        public Color Color;
        public Sprite ReadySprite;
        public List<Sprite> AnimationSprites;    
        public float AnimationFrequency;     
        public LayerMask Layer;
        public float Damage;
        public string State;
        private Animator stateAnimator;
        
        void Start()
        {
            stateAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMaskToLayer(Layer))
            {
                stateAnimator.SetTrigger("Damage");
                var otherHealthBehaviour = collider.gameObject.GetComponent<HealthBehaviour>();
                if(otherHealthBehaviour != null){
                    otherHealthBehaviour.TakeDamage(Damage);
                }
            }
        }

        private int LayerMaskToLayer(LayerMask layerMask) {
         int layerNumber = 0;
         int layer = layerMask.value;
         while(layer > 0) {
             layer = layer >> 1;
             layerNumber++;
         }
         return layerNumber - 1;
     }
    }
}