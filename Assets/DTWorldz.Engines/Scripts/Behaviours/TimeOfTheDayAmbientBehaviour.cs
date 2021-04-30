using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DTWorldz.Engines
{
    public class TimeOfTheDayAmbientBehaviour : TimeOfDayUIBehaviour
    {
        public Color[] AmbientHourColorsList;

        //private SpriteRenderer ambientSpriteRenderer;
        [SerializeField]
        private Light2D ambientLight;
        private Color targetColor;

        private float ratio;
        private float tweenLength;

        // Start is called before the first frame update
        void Start()
        {
            // ambientSpriteRenderer = GetComponent<SpriteRenderer>();
            // targetColor = ambientSpriteRenderer.color;
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
                // if (currentGameTime.Hours >= 6 && currentGameTime.Hours <= 18)
                // {
                //     targetColor = DayColor;
                //     tweenLength = ratio * 10 * 60;
                // }
                // else
                // {
                //     targetColor = NightColor;
                //     tweenLength = ratio * 14 * 60;
                // }

                // if (currentGameTime.Hours >= 0 && currentGameTime.Hours <= 4)
                // {
                //     targetColor = NightColor;
                //     tweenLength  = ratio * (4/24);
                // }
                // else 
                // if (currentGameTime.Hours > 4 && currentGameTime.Hours <= 9)
                // {
                //     targetColor = MorningColor;
                //     tweenLength  = ratio * (5/24);
                // }
                // else if (currentGameTime.Hours > 9 && currentGameTime.Hours <= 15)
                // {
                //     targetColor = DayColor;
                //     tweenLength  = ratio * (6/24);
                // }
                // else if (currentGameTime.Hours > 15 && currentGameTime.Hours <= 18)
                // {
                //     targetColor = DuskColor;
                //     tweenLength  = ratio * (3/24);
                // }
                // else if ( currentGameTime.Hours > 18 && currentGameTime.Hours <= 21)
                // {
                //     targetColor = NightColor;
                //     tweenLength  = ratio * (3/24);
                // }

            }
        }

        void Update()
        {
            //ambientSpriteRenderer.color = Color.Lerp(ambientSpriteRenderer.color, targetColor, /*tweenLength / */Time.deltaTime);
            if(ambientLight == null){
                return;
            }
            ambientLight.color = Color.Lerp(ambientLight.color, targetColor, /*tweenLength / */Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime);
        }
    }
}