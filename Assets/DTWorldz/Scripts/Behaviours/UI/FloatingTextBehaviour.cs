using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.UI
{
    public class FloatingTextBehaviour : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        private float thrust = 3.0f;
        public float DestroyAfterSeconds = 1f;
        void Start()
        {
            rb2D = gameObject.GetComponent<Rigidbody2D>();
            Destroy(gameObject, DestroyAfterSeconds);
        }

        void FixedUpdate()
        {
            rb2D.AddForce(transform.up * thrust);
        }
    }
}