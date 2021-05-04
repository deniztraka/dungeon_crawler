using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Engines.DayNightCycle;
using UnityEngine;
namespace DTWorldz.Engines.WeatherSystem
{
    public class WeatherSystem : MonoBehaviour
    {
        [SerializeField]
        private int toStopRaining;
        private Dictionary<int, float> MaxRainPossibilities = new Dictionary<int, float>(){
            {1,.075f},    // January
            {2,.1f},    // February
            {3,.3f},    // March
            {4,.5f},    // April
            {5,0f},    // May
            {6,0f},    // June
            {7,0f},    // July
            {8,0f},    // August
            {9,.1f},    // September
            {10,.3f},   // October
            {11,.1f},   // November
            {12,.075f},   // December
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
            toStopRaining--;
            if (toStopRaining < 0)
            {
                toStopRaining = 0;
            }
            else
            {
                RainBehaviour.StartRaining();
                return;
            }

            if (RainBehaviour != null && maxRainPossibility > 0 && toStopRaining == 0)
            {
                var rainingChance = UnityEngine.Random.value;
//                Debug.Log(rainingChance);
                UnityEngine.Random.InitState((int)Time.time);
                if (rainingChance < maxRainPossibility)
                {
                    if (toStopRaining == 0)
                    {
                        toStopRaining = UnityEngine.Random.Range(1, 6);
                    }
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