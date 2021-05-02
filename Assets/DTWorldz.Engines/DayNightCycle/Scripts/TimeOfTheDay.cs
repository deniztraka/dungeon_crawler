using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Engines.DayNightCycle
{


    public class GameTime
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;

        public GameTime(int days, int hours, int minutes, int seconds)
        {
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }
    }

    public class TimeOfTheDay : MonoBehaviour
    {
        public int DayLengthInSeconds;
        public bool isEnabled;
        public delegate void TimeOfTheDayHandler();
        public event TimeOfTheDayHandler OnAfterValueChangedEvent;

        private int currentDay;
        private int currentHour;
        private int currentMinute;
        private int currentSecond;
        private float processFrequencyInSeconds = 1;
        public long RealGameSecondsPast;
        private float currentTimeOfDay;

        void Start()
        {
            if (isEnabled)
            {
                //Init();
                StartCoroutine(Process());
            }
        }

        internal void SetCurrentTime(long realGameSecondsPast)
        {
            RealGameSecondsPast = realGameSecondsPast;
        }

        private void Init()
        {
            RealGameSecondsPast = 0;
        }

        public GameTime GetGameTime()
        {
            return new GameTime(currentDay, currentHour, currentMinute, currentSecond);
        }

        private IEnumerator Process()
        {
            while (isEnabled)
            {
                yield return new WaitForSeconds((float)processFrequencyInSeconds);
                CalculateTimeOfTheDay();
                RealGameSecondsPast++;
            }
        }

        public void CalculateTimeOfTheDay()
        {
            if (RealGameSecondsPast > 0)
            {

                var ratio = DayLengthInSeconds / 86400f;
                //how many seconds past according to game time.
                var secondsPastInGame = RealGameSecondsPast / ratio;

                var dayx = secondsPastInGame / (24 * 3600);

                secondsPastInGame = secondsPastInGame % (24 * 3600);
                var hourx = secondsPastInGame / 3600;

                secondsPastInGame %= 3600;
                var minutesx = secondsPastInGame / 60;

                secondsPastInGame %= 60;
                var secondsx = secondsPastInGame;

                currentDay = (int)dayx;
                currentHour = (int)hourx;
                currentMinute = (int)minutesx;
                currentSecond = (int)secondsx;

                if (OnAfterValueChangedEvent != null)
                {
                    OnAfterValueChangedEvent();
                }
            }
        }
    }
}