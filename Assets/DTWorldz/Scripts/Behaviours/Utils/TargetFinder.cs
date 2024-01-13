using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Models;
using UnityEngine;
using UnityEngine.PlayerLoop;
namespace DTWorldz.Behaviours.Utils
{
    public class TargetFinder : MonoBehaviour
    {
        public LayerMask LayerMask;

        public Vector2 ColliderAreaSizeUp;
        public Vector2 ColliderAreaOffsetUp;

        public Vector2 ColliderAreaSizeLeft;
        public Vector2 ColliderAreaOffsetLeft;

        public Vector2 ColliderAreaSizeDown;
        public Vector2 ColliderAreaOffsetDown;

        public Vector2 ColliderAreaSizeRight;
        public Vector2 ColliderAreaOffsetRight;
        private BoxCollider2D coll;
        private PlayerMovementBehaviour movementBehaviour;
        private Direction direction;

        private Interactable interactable;


        public delegate void TargetFinderEventHandler(Interactable interactable);
        public event TargetFinderEventHandler OnTargetChanged;

        [SerializeField]
        // Start is called before the first frame update
        void Start()
        {
            coll = GetComponent<BoxCollider2D>();
            movementBehaviour = transform.parent.GetComponent<PlayerMovementBehaviour>();
        }

        // Update is called once per frame
        void Update()
        {
            direction = movementBehaviour.GetDirection();
            switch (direction)
            {
                case Direction.Up:
                    coll.offset = ColliderAreaOffsetUp;
                    coll.size = ColliderAreaSizeUp;

                    break;
                case Direction.Left:
                    coll.offset = ColliderAreaOffsetLeft;
                    coll.size = ColliderAreaSizeLeft;
                    break;
                case Direction.Down:
                    coll.offset = ColliderAreaOffsetDown;
                    coll.size = ColliderAreaSizeDown;
                    break;
                case Direction.Right:
                    coll.offset = ColliderAreaOffsetRight;
                    coll.size = ColliderAreaSizeRight;
                    break;
            }
        }

        // void OnTriggerEnter2D(Collider2D col)
        // {
        //     Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        // }

        void FixedUpdate()
        {
            var colliders = Physics2D.OverlapBoxAll(coll.transform.position + new Vector3(coll.offset.x, coll.offset.y, 0), coll.size, 0f, LayerMask);
            if (colliders != null && colliders.Length > 0)
            {
                // get closest collider
                colliders = colliders.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToArray();
                var closestInteractable = colliders[0].gameObject.GetComponent<Interactable>();
                if(interactable != closestInteractable){
                    interactable = closestInteractable;
                    if (OnTargetChanged != null)
                    {
                        OnTargetChanged(interactable);
                    }
                }

            } else {
                interactable = null;
                if (OnTargetChanged != null)
                {
                    OnTargetChanged(null);
                }
            }
        }
    }
}