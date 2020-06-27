using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public float Speed = 1f;   //Movement Speed of the Player
        private Vector2 movement;           //Movement Axis
        private Rigidbody2D rigidbody2d;      //Player Rigidbody Component

        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = this.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            rigidbody2d.MovePosition(rigidbody2d.position + movement * Speed * Time.fixedDeltaTime);
        }
    }
}