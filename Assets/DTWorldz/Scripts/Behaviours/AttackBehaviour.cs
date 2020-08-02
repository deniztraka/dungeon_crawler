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
                    coll.offset = new Vector2(0, 1f);
                    break;
                case Direction.Left:
                    coll.offset = new Vector2(-.75f, .5f);
                    break;
                case Direction.Down:
                    coll.offset = new Vector2(0, -.5f);
                    break;
                case Direction.Right:
                    coll.offset = new Vector2(.75f, .5f);
                    break;
            }
        }

        internal void SetDirection(Direction direction)
        {
            this.direction = direction;
        }
    }

}