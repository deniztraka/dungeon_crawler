using System;
using System.Collections;
using System.Collections.Generic;
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

        [SerializeField]
        private Direction direction;
        private List<Vector3> paths; 
        private Tilemap wallMap;

        private float resultingSpeed = 1f;   //Movement Speed of the Player
        private bool isRunning = false;
        private bool attackingTrigger = false;
        [SerializeField]
        private float attackingFrequency = 0.5f;
        private float attackTime = 0;
        private Vector2 movement;           //Movement Axis
        private Rigidbody2D rigidbody2d;      //Player Rigidbody Component
        private Animator animator;           //animator
        private AttackBehaviour attackBehaviour;
        public List<Animator> AnimationSlots;

        // Start is called before the first frame update
        void Start()
        {
            var wallsObj = GameObject.Find("Walls");
            if(wallsObj != null){
                wallMap = wallsObj.GetComponent<Tilemap>();
            }
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
            attackingTrigger = false;

            if (wallMap != null && Input.GetMouseButtonDown(0))
            {
                paths = AStar.FindPath(wallMap, transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (paths != null && paths.Count > 0)
                {
                    Debug.Log(paths.Count);
                }
            }
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

            if (attackTime <= 0)
            {
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    attackingTrigger = true;
                    Attack();
                    attackTime = attackingFrequency;                    
                }

            }
            else
            {
                attackTime -= Time.deltaTime;
            }
        }

        private void Attack()
        {
            attackBehaviour.Attack();
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
            animator.SetBool("Attack", attackingTrigger);
            foreach (var animatorSlot in AnimationSlots)
            {
                animatorSlot.SetFloat("MovementSpeed", resultingSpeed);
                animatorSlot.SetBool("Attack", attackingTrigger);
            }
        }

        private void FixedUpdate()
        {
            rigidbody2d.MovePosition(rigidbody2d.position + movement * resultingSpeed * Time.fixedDeltaTime);
        }
    }
}