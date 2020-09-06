using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

namespace DTWorldz.Behaviours.Items.Deco
{
    [RequireComponent(typeof(Light2D))]
    public class FlickeringLightBehaviour : MonoBehaviour
    {
        private float timePassed = 0;
        private float minFrequency = .1f;
        private float maxFrequency = 2f;

        public Light2D light2D;
        public float MaxIntensity = 1.5f;
        public float MinIntensity = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            light2D = GetComponent<Light2D>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (timePassed > Random.Range(minFrequency, maxFrequency))
            {
                timePassed = 0;
                light2D.intensity = Random.Range(MinIntensity, MaxIntensity);
            }
            timePassed += Time.deltaTime;
        }
    }
}
