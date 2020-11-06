using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Models;
using EZCameraShake;
using UnityEngine;
using DTWorlds.Behaviours.Effects;
using DTWorldz.Behaviours.UI;

namespace DTWorldz.Behaviours
{
    public class AttackBehaviour : MonoBehaviour
    {
        public float Damage = 5;
        public bool IsPlayer;
        public float KnockbackForce;
        public float AttackRange;
        [SerializeField]
        private LayerMask layer = 0;
        private Direction direction;
        private BoxCollider2D coll;
        private Vector2 collSizeDirectionAddition;

        private AudioManager audioManager;

        public delegate bool AttackingHandler();
        public event AttackingHandler OnBeforeAttack;
        public event AttackingHandler OnAfterAttack;

        public Vector2 AttackingColliderSizeUp;
        public Vector2 AttackingColliderOffsetUp;

        public Vector2 AttackingColliderSizeLeft;
        public Vector2 AttackingColliderOffsetLeft;

        public Vector2 AttackingColliderSizeDown;
        public Vector2 AttackingColliderOffsetDown;

        public Vector2 AttackingColliderSizeRight;
        public Vector2 AttackingColliderOffsetRight;


        public float AttackingFrequency = 0.5f;

        [SerializeField]
        private float attackTime = 0;

        public float ActionPoint = 5;


        // Start is called before the first frame update
        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            audioManager = gameObject.GetComponent<AudioManager>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (direction)
            {
                case Direction.Up:
                    coll.offset = AttackingColliderOffsetUp;
                    coll.size = AttackingColliderSizeUp;

                    break;
                case Direction.Left:
                    coll.offset = AttackingColliderOffsetLeft;
                    coll.size = AttackingColliderSizeLeft;
                    break;
                case Direction.Down:
                    coll.offset = AttackingColliderOffsetDown;
                    coll.size = AttackingColliderSizeDown;
                    break;
                case Direction.Right:
                    coll.offset = AttackingColliderOffsetRight;
                    coll.size = AttackingColliderSizeRight;
                    break;
            }

            attackTime -= Time.deltaTime;
            attackTime = attackTime <= 0 ? 0 : attackTime;
        }

        public Vector2 GetSizeEdition()
        {
            return collSizeDirectionAddition;
        }

        internal void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        IEnumerator LateAttack(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (IsPlayer)
            {
                CameraShaker.Instance.ShakeOnce(1f, 0.5f, 0.1f, 0.1f);
            }
            var colliders = Physics2D.OverlapBoxAll(coll.transform.position + new Vector3(coll.offset.x, coll.offset.y, 0), coll.size, 0f, layer);
            if (colliders != null && colliders.Length > 0)
            {
                var firstCollider = colliders[0];
                var enemyHealthBehaviour = firstCollider.gameObject.GetComponent<HealthBehaviour>();
                if (enemyHealthBehaviour != null)
                {
                    audioManager.Play("Hit");
                    enemyHealthBehaviour.TakeDamage(Damage, DamageType.Physical);
                    
                    if (KnockbackForce > 0)
                    {
                        var difference = enemyHealthBehaviour.transform.position - this.transform.position;
                        difference = difference.normalized * KnockbackForce;
                        enemyHealthBehaviour.transform.position = new Vector2(enemyHealthBehaviour.transform.position.x + difference.x, enemyHealthBehaviour.transform.position.y + difference.y);
                    }
                    if (IsPlayer)
                    {
                        if (enemyHealthBehaviour.BodyType == BodyType.Flesh)
                        {
                            StartCoroutine(CreateBloodStainsAfterSeconds(0.35f));
                        }
                    }
                }
            }

            if (OnAfterAttack != null)
            {
                OnAfterAttack();
            }
        }

        IEnumerator CreateBloodStainsAfterSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            BloodStainsPool.Instance.Create(transform.position);
        }

        IEnumerator PlaySwing(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (audioManager != null)
            {
                audioManager.Play("Swing");
            }
        }

        public bool Attack()
        {
            if (attackTime <= 0)
            {
                if (OnBeforeAttack != null)
                {
                    var result = OnBeforeAttack();
                    if (!result)
                    {
                        return false;
                    }
                }
                StartCoroutine(PlaySwing(0.2f));
                StartCoroutine(LateAttack(0.5f));
                attackTime = AttackingFrequency;
                return true;
            }
            return false;
        }
    }
}