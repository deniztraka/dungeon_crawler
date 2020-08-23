﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementBehaviour : MonoBehaviour
    {
        public float Speed = 3f;   //Movement Speed 
        public float RunningSpeed = 5f;   //Movement Speed
        public List<Animator> AnimationSlots;

        [SerializeField]
        private Direction direction;


        private List<Vector3> paths;
        [SerializeField]
        private Tilemap wallMap;
        private float resultingSpeed = 1f;
        private bool isRunning = false;
        private bool attackingTrigger = false;
        private Vector2 movement;           //Movement Axis
        private Rigidbody2D rigidbody2d;      //Rigidbody Component
        private Animator animator;           //animator
        private AttackBehaviour attackBehaviour;
        private Vector3 targetPoint;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponent<Animator>();
            direction = Direction.Right;
        }

        public void SetMovementGrid(Tilemap wallMap)
        {
            this.wallMap = wallMap;
        }

        public void SetTargetPoint(Vector3 target)
        {
            targetPoint = target;
            if (wallMap != null)
            {
                paths = AStar.FindPath(wallMap, transform.position, targetPoint);
                if (paths != null && paths.Count > 1)
                {
                    paths.RemoveAt(0);
                    movement = (paths[0] - transform.position).normalized;
                    // Debug.Log(paths.Count);
                    // Debug.Log((paths[1] - transform.position).normalized);
                }
            }
        }

        internal void GoIdle()
        {
            paths = new List<Vector3>();
            targetPoint = transform.position;
        }


        internal Tilemap GetMovementMap()
        {
            return wallMap;
        }

        // Update is called once per frame
        void Update()
        {
            CheckMovementPaths();
            //HandleAnimations();


        }

        private void CheckMovementPaths()
        {

            if (paths != null && paths.Count > 0)
            {
                var colliders = Physics2D.OverlapCircleAll(paths[0], 0.5f);
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
                //Debug.Log(deg);

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
            if (animator != null)
            {
                if (movement != Vector2.zero)
                {
                    animator.SetFloat("Horizontal", movement.x);
                    animator.SetFloat("Vertical", movement.y);
                    foreach (var animator in AnimationSlots)
                    {
                        animator.SetFloat("Horizontal", movement.x);
                        animator.SetFloat("Vertical", movement.y);
                    }
                    resultingSpeed = isRunning ? RunningSpeed : Speed;

                    SetDirection(movement.x, movement.y);
                }
                else
                {
                    resultingSpeed = 0;
                }
                animator.SetFloat("MovementSpeed", resultingSpeed);
                animator.SetBool("Attack", attackingTrigger);
                foreach (var animatorSlot in AnimationSlots)
                {
                    animatorSlot.SetFloat("MovementSpeed", resultingSpeed);
                    animatorSlot.SetBool("Attack", attackingTrigger);
                }
            }
        }

        private void FixedUpdate()
        {
            rigidbody2d.MovePosition(rigidbody2d.position + movement * resultingSpeed * Time.fixedDeltaTime);
        }

        void OnDrawGizmosSelected()
        {
            if (paths != null && paths.Count > 0)
            {
                Gizmos.color = Color.yellow;
                foreach (var point in paths)
                {
                    Gizmos.DrawWireSphere(point, 0.5f);
                }

                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(paths[0], 0.5f);
            }

            // Gizmos.color = Color.blue;

            // Gizmos.DrawWireSphere(targetPoint, 0.5f);

        }
    }
}