using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using DTWorldz.Models;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.Mobiles
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementBehaviour : MonoBehaviour
    {
        public bool DrawGizmos = false;
        public float Speed = 3f;   //Movement Speed 
        public float RunningSpeed = 5f;   //Movement Speed


        public List<Animator> AnimationSlots;

        public GameObject FollowingTarget;

        [SerializeField]
        private Direction direction;
        private ObjectSpawnerBehaviour spawnerBehaviour;

        private List<Vector3> paths;
        [SerializeField]
        private Tilemap wallMap;
        [SerializeField]
        private float resultingSpeed = 1f;
        [SerializeField]
        private bool isRunning = false;
        [SerializeField]
        private bool isAttacking = false;



        private bool attackingTrigger = false;

        private Vector2 movement;           //Movement Axis
        private Rigidbody2D rigidbody2d;      //Rigidbody Component

        private AttackBehaviour attackBehaviour;
        private Vector3 targetPoint;
        private HealthBehaviour healthBehaviour;
        public float AwareDistance = 5f;
        public float CloseDistance = 3f;

        internal Direction GetDirection()
        {
            return direction;
        }

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = this.GetComponent<Rigidbody2D>();
            //animator = this.GetComponent<Animator>();
            direction = Direction.Right;
            attackBehaviour = GetComponentInChildren<AttackBehaviour>();
            healthBehaviour = this.GetComponent<HealthBehaviour>();
            healthBehaviour.OnDeath += new HealthChanged(TriggerDeath);
            FollowingTarget = null;
            if (attackBehaviour != null)
            {
                attackBehaviour.OnBeforeAttack += new AttackBehaviour.AttackingHandler(BeforeAttack);
                attackBehaviour.OnAfterAttack += new AttackBehaviour.AttackingHandler(AfterAttack);
            }
        }

        private bool AfterAttack()
        {
            isAttacking = false;
            return true;
        }

        private bool BeforeAttack()
        {
            isAttacking = true;
            return true;
        }

        void TriggerDeath(float currentHealth, float maxHealth)
        {
            paths = new List<Vector3>();
            movement = Vector2.zero;
            foreach (var anim in AnimationSlots)
            {
                anim.SetTrigger("Dead");
            }
        }

        public void SetSpawner(ObjectSpawnerBehaviour spawner)
        {
            this.spawnerBehaviour = spawner;
        }

        public ObjectSpawnerBehaviour GetSpawner()
        {
            return spawnerBehaviour;
        }

        internal void SetTargetPaths()
        {
            if (FollowingTarget != null)
            {
                SetTargetPoint(FollowingTarget.transform.position);
            }
        }

        public void SetFollowingTarget(GameObject target)
        {
            FollowingTarget = target;
            SetTargetPaths();
            //animator.SetTrigger("Follow");
        }

        internal object GetFollowingTarget()
        {
            return FollowingTarget;
        }

        public void SetMovementGrid(Tilemap wallMap)
        {
            this.wallMap = wallMap;
        }

        internal void SetTargetPoint(Vector3 target)
        {
            targetPoint = target;
            if (wallMap != null)
            {
                paths = AStar.FindPath(wallMap, transform.position, targetPoint);
                if (paths != null && paths.Count > 0)
                {
                    paths[paths.Count - 1] = targetPoint;
                }

                if (paths != null && paths.Count > 1)
                {
                    paths.RemoveAt(0);
                    movement = (paths[0] - transform.position).normalized;
                }
            }
        }

        public void SetIsRunning(bool isRunning)
        {
            this.isRunning = isRunning;

        }

        internal void GoIdle()
        {
            paths = new List<Vector3>();
            targetPoint = transform.position;
            isRunning = false;
        }

        internal Tilemap GetMovementMap()
        {
            return wallMap;
        }

        // Update is called once per frame
        void Update()
        {
            CheckMovementPaths();
            HandleAnimations();
            if (attackBehaviour != null)
            {
                attackBehaviour.SetDirection(direction);
            }
            // if (Input.GetMouseButtonDown(0))
            // {
            //     SetFollowingTarget(GameObject.FindGameObjectWithTag("Player"));
            // }     
            //movement = Vector2.zero;       
        }

        private void CheckMovementPaths()
        {

            if (paths != null && paths.Count > 0)
            {
                var colliders = Physics2D.OverlapCircleAll(paths[0], 0.25f);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject == gameObject)
                    {
                        paths.RemoveAt(0);
                        if (paths.Count > 0)
                        {
                            movement = (paths[0] - transform.position).normalized;
                        }
                        else
                        {
                            movement = Vector2.zero;
                        }
                        break;
                    }
                }
            }
            else
            {
                movement = Vector2.zero;
            }
        }

        private void SetDirection(float x, float y)
        {
            if (paths != null && paths.Count > 0)
            {
                var deltaX = transform.position.x - paths[0].x;
                var deltaY = transform.position.y - paths[0].y;
                var rad = Math.Atan2(deltaY, deltaX);
                var deg = rad * (180 / Math.PI);

                if (deg > -45 && deg < 45)
                {
                    direction = Direction.Left;
                }
                else if (deg >= 45 && deg <= 135)
                {
                    direction = Direction.Down;
                }
                else if ((deg >= 135 && deg <= 180) || (deg <= -135 && deg >= -180))
                {
                    direction = Direction.Right;
                }
                else if ((deg < -45 && deg > -180))
                {
                    direction = Direction.Up;
                }
            }
        }

        private void HandleAnimations()
        {
            if (movement != Vector2.zero)
            {

                // animator.SetFloat("Horizontal", movement.x);
                // animator.SetFloat("Vertical", movement.y);
                foreach (var anim in AnimationSlots)
                {
                    anim.SetFloat("Horizontal", movement.x);
                    anim.SetFloat("Vertical", movement.y);
                }
                resultingSpeed = isRunning ? RunningSpeed : Speed;

                SetDirection(movement.x, movement.y);
            }
            else
            {
                resultingSpeed = 0;
            }

            // animator.SetFloat("MovementSpeed", resultingSpeed);
            // animator.SetBool("Attack", attackingTrigger);
            foreach (var animatorSlot in AnimationSlots)
            {
                animatorSlot.SetFloat("MovementSpeed", resultingSpeed);
                // if (animatorSlot.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                // {
                animatorSlot.SetBool("Attack", isAttacking);
                // }
            }

        }

        public float GetResultingSpeed()
        {
            return resultingSpeed;
        }

        private void FixedUpdate()
        {
            resultingSpeed = isRunning ? RunningSpeed : Speed;
            rigidbody2d.MovePosition(rigidbody2d.position + movement * resultingSpeed * Time.fixedDeltaTime);
        }

        void OnDrawGizmosSelected()
        {
            if (DrawGizmos)
            {
                if (paths != null && paths.Count > 0)
                {
                    Gizmos.color = Color.yellow;
                    foreach (var point in paths)
                    {
                        Gizmos.DrawWireSphere(point, 0.25f);
                    }

                    Gizmos.color = Color.red;

                    Gizmos.DrawWireSphere(paths[0], 0.25f);
                }
            }
        }
    }
}