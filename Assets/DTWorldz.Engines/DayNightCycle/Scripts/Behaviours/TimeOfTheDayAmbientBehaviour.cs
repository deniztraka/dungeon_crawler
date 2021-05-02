using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DTWorldz.Engines.DayNightCycle
{
    public class TimeOfTheDayAmbientBehaviour : TimeOfDayUIBehaviour
    {
        public Color[] AmbientHourColorsList;
        [SerializeField]
        private Light2D ambientLight;
        private Color targetColor;

        private float ratio;
        private float tweenLength;

        void Start()
        {
            ambientLight = GetComponent<Light2D>();
            targetColor = ambientLight.color;
            ratio = TimeOfTheDay.DayLengthInSeconds / 86400f;

            if (TimeOfTheDay != null)
            {
                targetColor = AmbientHourColorsList[TimeOfTheDay.GetGameTime().Hours];
            }
        }
        public override void UpdateMe()
        {
            tweenLength = 0f;
            if (TimeOfTheDay != null)
            {
                var currentGameTime = TimeOfTheDay.GetGameTime();
                targetColor = AmbientHourColorsList[currentGameTime.Hours];
            }
        }
        

        void Update()
        {
            //ambientSpriteRenderer.color = Color.Lerp(ambientSpriteRenderer.color, targetColor, /*tweenLength / */Time.deltaTime);
            if (ambientLight == null)
            {
                return;
            }
            ambientLight.color = Color.Lerp(ambientLight.color, targetColor, /*tweenLength / */Time.deltaTime);
            
            
            
        }
    }
}