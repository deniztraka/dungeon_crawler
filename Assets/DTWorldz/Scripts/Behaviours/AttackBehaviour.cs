using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Models;
using EZCameraShake;
using UnityEngine;

namespace DTWorldz.Behaviours
{
    public class AttackBehaviour : MonoBehaviour
    {
        public bool IsPlayer;
        public float KnockbackForce;
        public float AttackRange;
        [SerializeField]
        private LayerMask layer = 0;
        private Direction direction;
        private BoxCollider2D coll;
        private Vector2 collSizeDirectionAddition;

        private AudioManager audioManager;


        public float AttackingFrequency = 0.5f;

        [SerializeField]
        private float attackTime = 0;


        // Start is called before the first frame update
        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            collSizeDirectionAddition = Vector2.zero;
            audioManager = gameObject.GetComponent<AudioManager>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (direction)
            {
                case Direction.Up:
                    coll.offset = new Vector2(0, 1f + AttackRange);
                    collSizeDirectionAddition = new Vector2(0.5f, 0f);
                    break;
                case Direction.Left:
                    coll.offset = new Vector2(-.75f - AttackRange, .5f);
                    collSizeDirectionAddition = new Vector2(0.5f, 0.5f);
                    break;
                case Direction.Down:
                    coll.offset = new Vector2(0, -.25f - AttackRange);
                    collSizeDirectionAddition = new Vector2(0.5f, 0f);
                    break;
                case Direction.Right:
                    coll.offset = new Vector2(.75f + AttackRange, .5f);
                    collSizeDirectionAddition = new Vector2(0.5f, 0.5f);
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
            var colliders = Physics2D.OverlapBoxAll(coll.transform.position + new Vector3(coll.offset.x, coll.offset.y, 0), coll.size + collSizeDirectionAddition, 0f, layer);
            if (colliders != null && colliders.Length > 0)
            {
                var firstCollider = colliders[0];
                var enemyHealthBehaviour = firstCollider.gameObject.GetComponent<HealthBehaviour>();
                if (enemyHealthBehaviour != null)
                {
                    //audioManager.Play("Hit");
                    enemyHealthBehaviour.TakeDamage(5, DamageType.Physical);
                    if (KnockbackForce > 0)
                    {
                        var difference = enemyHealthBehaviour.transform.position - this.transform.position;
                        difference = difference.normalized * KnockbackForce;
                        enemyHealthBehaviour.transform.position = new Vector2(enemyHealthBehaviour.transform.position.x + difference.x, enemyHealthBehaviour.transform.position.y + difference.y);
                    }
                }
            }
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
                StartCoroutine(PlaySwing(0.2f));
                StartCoroutine(LateAttack(0.5f));
                attackTime = AttackingFrequency;
                return true;
            }
            return false;
        }
    }
}