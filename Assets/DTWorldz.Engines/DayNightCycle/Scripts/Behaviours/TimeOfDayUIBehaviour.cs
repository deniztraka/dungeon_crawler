using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Engines.DayNightCycle
{
    public abstract class TimeOfDayUIBehaviour : MonoBehaviour
    {
        public TimeOfTheDay TimeOfTheDay;

        // Start is called before the first frame update
        void Awake()
        {
            if (TimeOfTheDay != null)
            {
                TimeOfTheDay.OnAfterValueChangedEvent += new TimeOfTheDay.TimeOfTheDayHandler(UpdateMe);
            }
        }
        public abstract void UpdateMe();
    }
}
