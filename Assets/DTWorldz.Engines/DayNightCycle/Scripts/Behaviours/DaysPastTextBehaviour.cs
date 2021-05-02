using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Engines.DayNightCycle
{
    public class DaysPastTextBehaviour : TimeOfDayUIBehaviour
    {
        public string Format;

        // Start is called before the first frame update
        void Start()
        {
            if (String.IsNullOrEmpty(Format))
            {
                Format = "{0}-{1}-{2}, {3}:{4}:{5}";
            }
        }

        public override void UpdateMe()
        {
            if (TimeOfTheDay != null)
            {
                var currentGameTime = TimeOfTheDay.GetGameTime();
                var textComp = GetComponent<Text>();
                textComp.text = String.Format(Format,
                currentGameTime.Years,
                currentGameTime.Months,
                currentGameTime.Days,
                currentGameTime.Hours.ToString().Length == 1 ? ("0" + currentGameTime.Hours.ToString()) : currentGameTime.Hours.ToString(),
                currentGameTime.Minutes.ToString().Length == 1 ? ("0" + currentGameTime.Minutes.ToString()) : currentGameTime.Minutes.ToString(),
                currentGameTime.Seconds
                
                );
            }
        }
    }
}
