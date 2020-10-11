using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.UI
{
    public class FloatingTextBehaviour : MonoBehaviour
    {
        protected Rigidbody2D Rb2D;
        private float thrust = 3.0f;
        public float DestroyAfterSeconds = 1f;
        protected Vector3 ForceDirection;
        private TextMesh textMesh;
        void Start()
        {
            
            Rb2D = gameObject.GetComponent<Rigidbody2D>();
            ForceDirection = new Vector3(UnityEngine.Random.Range(-1f, 1.01f), 1, 0);
            ForceDirection = ForceDirection.normalized;
            Destroy(gameObject, DestroyAfterSeconds);
        }

        void FixedUpdate()
        {
            Rb2D.AddForce(ForceDirection * thrust);
        }

        public virtual void SetText(string text)
        {

            textMesh = gameObject.GetComponent<TextMesh>();
            if (this.textMesh)
            {
                textMesh.text = text;
            }
        }
    }
}