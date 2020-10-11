using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.UI
{
    public class FloatingGoldLootTextBehaviour : FloatingTextBehaviour
    {

        void Start()
        {

            Rb2D = gameObject.GetComponent<Rigidbody2D>();
            ForceDirection = new Vector3(UnityEngine.Random.Range(-0.25f, 0.251f), 1, 0);
            ForceDirection = ForceDirection.normalized;
            Destroy(gameObject, DestroyAfterSeconds);
        }
        public override void SetText(string text)
        {
            base.SetText(text + " gp");
        }
    }
}