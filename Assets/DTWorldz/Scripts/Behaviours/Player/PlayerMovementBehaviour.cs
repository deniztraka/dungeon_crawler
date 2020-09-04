﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Models;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public float Speed = 3f;   //Movement Speed of the Player
        public float RunningSpeed = 5f;   //Movement Speed of the Player
        public bool ClickAndGoEnabled = false;

        [SerializeField]
        private Direction direction;
        private List<Vector3> paths;
        private Tilemap wallMap;

        private float resultingSpeed = 1f;   //Movement Speed of the Player
        private bool isRunning = false;
        private bool attackingTrigger = false;
        [SerializeField]
        private float attackingFrequency = 0.5f;
        [SerializeField]
        private float attackTime = 0;
        private Vector2 movement;           //Movement Axis
        private Rigidbody2D rigidbody2d;      //Player Rigidbody Component
        private Animator animator;           //animator
        private AttackBehaviour attackBehaviour;
        private AudioManager audioManager;
        public List<Animator> AnimationSlots;

        public Joystick Joystick;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponent<Animator>();
            audioManager = gameObject.GetComponent<AudioManager>();
            attackBehaviour = transform.GetComponentInChildren<AttackBehaviour>();
            direction = Direction.Right;
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            //CheckMovementPaths();
            HandleAnimations();
            attackBehaviour.SetDirection(direction);
            //reset attacking trigger
            attackingTrigger = false;
        }

        private void HandleInput()
        {
            if (Joystick != null)
            {
                movement.x = Joystick.Direction.x;
                movement.y = Joystick.Direction.y;
            }
            else
            {
                movement.x = Input.GetAxisRaw("Horizontal");
                movement.y = Input.GetAxisRaw("Vertical");
            }

            // if (Input.GetKeyUp(KeyCode.LeftShift))
            // {
            //     isRunning = false;
            // }
            // else if (Input.GetKeyDown(KeyCode.LeftShift))
            // {
            //     isRunning = true;
            // }

            isRunning = Joystick.IsAtAtMax;

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Attack();
            }

            attackTime -= Time.deltaTime;
            attackTime = attackTime <= 0 ? 0 : attackTime;
        }

        private void TriggerAttack()
        {
            attackBehaviour.Attack();
            attackTime = attackingFrequency;
        }

        public void Attack()
        {
            if (attackTime <= 0)
            {
                TriggerAttack();
                attackingTrigger = true;
            }
        }

        private float GetAngle()
        {
            return 1f;
        }

        private void SetDirection(float x, float y)
        {
            if (ClickAndGoEnabled && paths != null && paths.Count > 0)
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
            else if (!ClickAndGoEnabled)
            {

                if (x < 0)
                {
                    direction = Direction.Left;
                }
                else if (x > 0)
                {
                    direction = Direction.Right;
                }
                else
                {
                    if (y < 0)
                    {
                        direction = Direction.Down;
                    }
                    else
                    {
                        direction = Direction.Up;
                    }
                }
            }
        }

        private void HandleAnimations()
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

        public void SetMovementGrid(Tilemap wallMap)
        {
            this.wallMap = wallMap;
        }

        private void FixedUpdate()
        {
            PlayMovementSounds();
            rigidbody2d.MovePosition(rigidbody2d.position + movement * resultingSpeed * Time.fixedDeltaTime);
        }

        private void PlayMovementSounds()
        {
            if (audioManager != null)
            {
                if (isRunning)
                {
                    audioManager.Play("Walking");
                    audioManager.SetCurrentPitch(0.6f);
                }
                else if (movement.magnitude > 0)
                {
                    audioManager.Play("Walking");
                    audioManager.SetCurrentPitch(0.5f);
                }

                if(movement.magnitude == 0)
                {
                    audioManager.SetCurrentPitch(0.5f);
                    audioManager.Stop("Walking"); 
                }
                //Debug.Log(movement.magnitude);

                // if (movement.magnitude > 0 && !isRunning)
                // {

                //     audioManager.Stop("Running");
                //     audioManager.Play("Walking");
                // }
                // else
                // if (movement.magnitude > 0 && isRunning)
                // {
                //     audioManager.Play("Running");
                // }
                // else if (movement.magnitude == 0)
                // {
                //     audioManager.Stop("Walking");
                //     audioManager.Stop("Running");
                // }
            }
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
        }
    }
}