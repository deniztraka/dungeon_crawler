using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEngine;
namespace DTWorldz.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public float Speed = 3f;   //Movement Speed of the Player
        public float RunningSpeed = 5f;   //Movement Speed of the Player

        [SerializeField]
        private Direction direction;

        private float resultingSpeed = 1f;   //Movement Speed of the Player
        private bool isRunning = false;
        private bool isAttacking = false;
        private Vector2 movement;           //Movement Axis
        private Rigidbody2D rigidbody2d;      //Player Rigidbody Component
        private Animator animator;           //animator
        private AttackBehaviour attackBehaviour;
        public List<Animator> AnimationSlots;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = this.GetComponent<Rigidbody2D>();
            animator = this.GetComponent<Animator>();
            attackBehaviour = transform.GetComponentInChildren<AttackBehaviour>();
            direction = Direction.Right;
        }

        // Update is called once per frame
        void Update()
        {
            HandleInput();
            HandleAnimations();
            attackBehaviour.SetDirection(direction);
            //reset attacking trigger
            isAttacking = false;
        }

        private void HandleInput()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                isRunning = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isAttacking = true;
            }
        }

        private void SetDirection(float x, float y)
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
            animator.SetBool("Attack", isAttacking);
            foreach (var animatorSlot in AnimationSlots)
            {
                animatorSlot.SetFloat("MovementSpeed", resultingSpeed);
                animatorSlot.SetBool("Attack", isAttacking);
            }
        }

        private void FixedUpdate()
        {
            rigidbody2d.MovePosition(rigidbody2d.position + movement * resultingSpeed * Time.fixedDeltaTime);
        }
    }
}