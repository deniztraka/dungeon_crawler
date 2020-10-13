using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
namespace DTWorldz.Behaviours.Items.Deco
{
    public class TrapBehaviour : MonoBehaviour
    {
        public delegate void TrapStateHandler(string stateName);
        public event TrapStateHandler OnStateChanged;
        public int RefreshTime;
        public DamageType DamageType;
        public Color Color;
        public Sprite ReadySprite;
        public List<Sprite> AnimationSprites;
        public float AnimationFrequency;
        public LayerMask LayerMask;
        public float Damage;
        [SerializeField]
        private string state;
        public string State
        {
            get { return state; }
            set
            {
                if (!state.Equals(value))
                {
                    if(OnStateChanged != null){
                        OnStateChanged(value);
                    }
                }
                state = value;
            }
        }

        private Animator stateAnimator;
        private AudioManager audioManager;

        public virtual void Start()
        {
            stateAnimator = GetComponent<Animator>();
            audioManager = GetComponent<AudioManager>();
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (LayerMask == (LayerMask | (1 << collider.gameObject.layer)))
            {
                stateAnimator.SetTrigger("Damage");
                if (audioManager != null)
                {
                    audioManager.Play("Trigger");
                }
                var otherHealthBehaviour = collider.gameObject.GetComponent<HealthBehaviour>();
                if (otherHealthBehaviour != null)
                {
                    TakeDamage(otherHealthBehaviour);
                }
            }
        }

        protected virtual void TakeDamage(HealthBehaviour healthBehaviour)
        {
            healthBehaviour.TakeDamage(Damage, DamageType);
        }
    }
}