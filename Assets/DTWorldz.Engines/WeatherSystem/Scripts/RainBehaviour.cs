using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using UnityEngine;
namespace DTWorldz.Engines.WeatherSystem
{
    public class RainBehaviour : MonoBehaviour
    {
        private const float maxIntensity = 250f;
        [SerializeField]
        float changePeriod = 0;
        [SerializeField]
        float currentIntensity = 0;
        [SerializeField]
        float targetIntensity = 0;
        float startIntensity = 0;

        public bool IsRaining
        {
            get { return currentIntensity > 0; }
        }

        public float CurrentIntensity
        {
            get { return currentIntensity; }
        }

        private float t = 0;
        private AudioManager audioManager;

        private ParticleSystem rainParticles;
        private ParticleSystem.EmissionModule emissionModule;
        void Start()
        {
            rainParticles = GetComponent<ParticleSystem>();
            emissionModule = rainParticles.emission;
            audioManager = GetComponent<AudioManager>();
        }

        void Update()
        {
            // if (Input.GetMouseButtonUp(0))
            // {
            //     if (IsRaining)
            //     {
            //         StopRaining();
            //     }
            //     else
            //     {
            //         StartRaining();
            //     }
            // }

            if (currentIntensity == targetIntensity)
            {
                return;
            }

            t += Time.deltaTime / changePeriod;
            currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, t);

            emissionModule.rateOverTime = currentIntensity;

            if (audioManager != null)
            {
                audioManager.SetVolume(currentIntensity / maxIntensity * .25f);
            }
        }

        public void StartRaining()
        {
            var randomIntensity = Random.Range(0, maxIntensity);
            Random.InitState((int)Time.time);
            var randomPeriod = Random.Range(0, 10);
            UpdateIntensity(randomIntensity, randomPeriod);
        }

        private void UpdateIntensity(float intensity, float period)
        {
            t = 0;
            targetIntensity = intensity;
            changePeriod = period;
            startIntensity = currentIntensity;
        }

        public void StopRaining()
        {
            if (IsRaining)
            {
                var randomPeriod = Random.Range(0, 10);
                UpdateIntensity(0, randomPeriod);
            }
        }
    }
}