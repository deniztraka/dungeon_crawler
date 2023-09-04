using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DTWorldz.Engines.DayNightCycle
{
    public class TimeOfTheDayAmbientBehaviour : TimeOfDayUIBehaviour
    {
        public Color[] AmbientHourColorsList;
        [SerializeField]
        private UnityEngine.Rendering.Universal.Light2D ambientLight;
        private Color targetColor;
        private Color startColor;

        private float ratio;
        private float duration;
        private float t;

        void Start()
        {
            ambientLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            
            if (TimeOfTheDay != null)
            {
                targetColor = AmbientHourColorsList[TimeOfTheDay.GetGameTime().Hours];
                ambientLight.color = targetColor;
                startColor = ambientLight.color;
                duration = TimeOfTheDay.DayLengthInSeconds / 24;
                TimeOfTheDay.OnAfterHourChanged += new TimeOfTheDay.TimeOfTheDayHandler(HourChanged);
            }
        }

        private void HourChanged()
        {

            if (TimeOfTheDay != null && ambientLight != null)
            {
                t = 0f;
                var currentGameTime = TimeOfTheDay.GetGameTime();
                targetColor = AmbientHourColorsList[currentGameTime.Hours];
                startColor = ambientLight.color;
            }
        }

        public override void UpdateMe()
        {

        }


        void Update()
        {
            if (ambientLight == null)
            {
                return;
            }
            ambientLight.color = Color.Lerp(startColor, targetColor, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
        }
    }
}