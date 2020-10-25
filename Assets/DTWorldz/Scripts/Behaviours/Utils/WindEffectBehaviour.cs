using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Behaviours.Utils
{
    public class WindEffectBehaviour : MonoBehaviour
    {
        public float EffectIntensityMultiplier = 1f;
        private Material material;
        private float currentIntensity;
        private float currentSpeed;
        // Start is called before the first frame update
        void Start()
        {

            material = gameObject.GetComponent<SpriteRenderer>().material;
            currentIntensity = material.GetFloat("_WindIntensity");
            currentSpeed = material.GetFloat("_WindSpeed");
            var newIntensity = currentIntensity * EffectIntensityMultiplier;
            material.SetFloat("_WindIntensity", newIntensity);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnTriggerEnter2D(Collider2D collider)
        {


            if (collider.tag == "Player" || collider.tag == "Mobile")
            {
                if (currentSpeed < 2.5f)
                {
                    material.SetFloat("_WindSpeed", 2.5f);
                    StartCoroutine("OnAfter");
                }
            }
        }

        IEnumerator OnAfter()
        {

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(0.5f);

            material.SetFloat("_WindSpeed", currentSpeed);
        }
    }
}