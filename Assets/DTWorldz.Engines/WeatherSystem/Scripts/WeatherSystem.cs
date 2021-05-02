using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Engines.DayNightCycle;
using UnityEngine;
namespace DTWorldz.Engines.WeatherSystem
{
    public class WeatherSystem : MonoBehaviour
    {
        private Dictionary<int, float> MaxRainPossibilities = new Dictionary<int, float>(){
            {1,.2f},
            {2,.3f},
            {3,.5f},
            {4,.7f},
            {5,.4f},
            {6,.1f},
            {7,.0f},
            {8,.0f},
            {9,.3f},
            {10,.5f},
            {11,.2f},
            {12,.2f},
        };

        [SerializeField]
        private float maxRainPossibility;
        public RainBehaviour RainBehaviour;

        public TimeOfTheDay TimeOfTheDay;
        void Start()
        {
            if (TimeOfTheDay != null)
            {
                TimeOfTheDay.OnAfterHourChanged += new TimeOfTheDay.TimeOfTheDayHandler(HourChanged);
                // TimeOfTheDay.OnAfterDayChanged += new TimeOfTheDay.TimeOfTheDayHandler(DayChanged);
                TimeOfTheDay.OnAfterMonthChanged += new TimeOfTheDay.TimeOfTheDayHandler(MonthChanged);
                SetRainPossibility();
            }
        }

        private void HourChanged()
        {
            
            if (RainBehaviour != null && maxRainPossibility > 0)
            {
                var rainingChance = UnityEngine.Random.value;
                if (rainingChance < maxRainPossibility)
                {
                    RainBehaviour.StartRaining();
                }
                else
                {
                    RainBehaviour.StopRaining();
                }
            }
            else if (RainBehaviour != null)
            {
                RainBehaviour.StopRaining();
            }
        }

        private void MonthChanged()
        {
            SetRainPossibility();
        }

        // private void DayChanged()
        // {

        // }

        private void SetRainPossibility()
        {
            var possibility = 0f;
            MaxRainPossibilities.TryGetValue(TimeOfTheDay.GetGameTime().Months, out possibility);
            maxRainPossibility = possibility;
        }
    }
}