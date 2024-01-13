using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Engines.DayNightCycle
{
    public class ShadowsBehaviour : MonoBehaviour
    {

        private TimeOfTheDay timeOfTheDay;

        public SpriteRenderer renderer2d { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            timeOfTheDay = FindFirstObjectByType<TimeOfTheDay>();
            timeOfTheDay.OnAfterValueChangedEvent += new TimeOfTheDay.TimeOfTheDayHandler(ProcessShadows);
            ProcessShadows();
        }

        private void ProcessShadows()
        {
            if(this == null || gameObject == null || gameObject.activeSelf == false || !isActiveAndEnabled){
                return;
            }
            // Get the current game time
            var currentGameTime = timeOfTheDay.GetGameTime();

            // Calculate the new scale and rotation for the shadow based on currentGameTime
            Vector3 newScale = CalculateShadowScale(currentGameTime.Hours);
            Quaternion newRotation = CalculateShadowRotation(currentGameTime.Hours);

            // Apply the new scale and rotation to the shadow sprite
            transform.localScale = newScale;
            transform.rotation = newRotation;
        }

        private Vector3 CalculateShadowScale(int hour)
        {
            float scaleMultiplier;

            if (hour <= 12)
            {
                // Linearly decrease from 2 at hour 0 to 0.5 at hour 12
                scaleMultiplier = 2 - (hour / 12.0f) * 1.5f;
            }
            else
            {
                // Linearly increase from 0.5 at hour 12 to 2 at hour 24
                scaleMultiplier = 0.5f + ((hour - 12) / 12.0f) * 1.5f;
            }

            // Apply this multiplier to the Y scale
            return new Vector3(0.75f, scaleMultiplier, 1);
        }



        // The rotation function remains the same


        private Quaternion CalculateShadowRotation(int hour)
        {
            // Define the range of rotation
            float minRotation = -70f; // Minimum rotation angle
            float maxRotation = -50f; // Maximum rotation angle

            float rotationAngle;

            if (hour <= 12)
            {
                // From 0 to 12 hours, interpolate from minRotation to maxRotation
                rotationAngle = Mathf.Lerp(minRotation, maxRotation, hour / 12.0f);
            }
            else
            {
                // From 12 to 24 hours, interpolate from maxRotation back to minRotation
                rotationAngle = Mathf.Lerp(maxRotation, minRotation, (hour - 12) / 12.0f);
            }

            return Quaternion.Euler(0, 0, rotationAngle);
        }





        private void OnDestroy()
        {
            Debug.Log("ShadowsBehaviour.OnDestroy");
            // Unsubscribe to prevent memory leaks
            timeOfTheDay.OnAfterHourChanged -= new TimeOfTheDay.TimeOfTheDayHandler(ProcessShadows);
        }
    }
}