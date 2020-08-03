using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEngine;

namespace DTWorldz.Behaviours
{
    public class AttackBehaviour : MonoBehaviour
    {        
        public float AttackRange;
        [SerializeField]
        private LayerMask layer = 0;
        private Direction direction;
        private BoxCollider2D coll;

        // Start is called before the first frame update
        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            switch (direction)
            {
                case Direction.Up:
                    coll.offset = new Vector2(0, 1f + AttackRange);
                    break;
                case Direction.Left:
                    coll.offset = new Vector2(-.75f - AttackRange, .5f);
                    break;
                case Direction.Down:
                    coll.offset = new Vector2(0, -.5f - AttackRange);
                    break;
                case Direction.Right:
                    coll.offset = new Vector2(.75f + AttackRange, .5f);
                    break;
            }
        }

        internal void SetDirection(Direction direction)
        {
            this.direction = direction;
        }

        internal void Attack()
        {
            var colliders = Physics2D.OverlapBoxAll(coll.transform.position + new Vector3(coll.offset.x, coll.offset.y, 0), coll.size, 0f, layer);          
        }
    }

}