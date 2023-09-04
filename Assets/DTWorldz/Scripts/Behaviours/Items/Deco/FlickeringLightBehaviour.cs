using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace DTWorldz.Behaviours.Items.Deco
{
    [RequireComponent(typeof(UnityEngine.Rendering.Universal.Light2D))]
    public class FlickeringLightBehaviour : MonoBehaviour
    {
        private float timePassed = 0;
        private float minFrequency = .1f;
        private float maxFrequency = 2f;

        public UnityEngine.Rendering.Universal.Light2D light2D;
        public float MaxIntensity = 1.25f;
        public float MinIntensity = 0.75f;

        [SerializeField]
        private float currentMaxIntensity;
        [SerializeField]
        private float currentMinIntensity;

        // Start is called before the first frame update
        void Start()
        {
            light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        void LateUpdate()
        {
            if (timePassed > Random.Range(minFrequency, maxFrequency))
            {
                timePassed = 0;
                light2D.intensity = Random.Range(currentMinIntensity, currentMaxIntensity);
            }
            timePassed += Time.deltaTime;
        }

        internal void SetIntensity(float currentHealth, float maxHealth)
        {
            var rate = currentHealth / maxHealth;
            if (currentHealth <= 0)
            {
                rate = 0;
            }

            currentMinIntensity = MinIntensity * rate;
            currentMaxIntensity = MaxIntensity * rate;
        }
    }
}
