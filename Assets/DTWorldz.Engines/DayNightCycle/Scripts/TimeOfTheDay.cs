using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Engines.DayNightCycle
{


    public class GameTime
    {
        public int Years;
        public int Months;
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;

        public GameTime(int years, int months, int days, int hours, int minutes, int seconds)
        {
            // adding one more to to show the date we are in
            Years = years + 1;
            Months = months + 1;
            Days = days + 1;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
        }
    }

    public class TimeOfTheDay : MonoBehaviour
    {
        private const int RealSecondsInAMinute = 60;
        private const int RealSecondsInAnHour = 3600;
        private const int RealSecondsInADay = 86400;
        private const int RealSecondsInAMonth = 2592000;
        private const int RealSecondsInAYear = 31104000;

        public int DayLengthInSeconds;
        public bool isEnabled;
        public delegate void TimeOfTheDayHandler();
        public event TimeOfTheDayHandler OnAfterValueChangedEvent;
        public event TimeOfTheDayHandler OnAfterDayChanged;
        public event TimeOfTheDayHandler OnAfterMonthChanged;
        public event TimeOfTheDayHandler OnAfterYearChanged;
        public event TimeOfTheDayHandler OnAfterHourChanged;

        [SerializeField]
        private int currentYear;

        [SerializeField]
        private int currentMonth;
        [SerializeField]
        private int currentDay;
        [SerializeField]
        private int currentHour;
        [SerializeField]
        private int currentMinute;
        [SerializeField]
        private int currentSecond;
        [SerializeField]
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
            return new GameTime(currentYear, currentMonth, currentDay, currentHour, currentMinute, currentSecond);
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

                var ratio = DayLengthInSeconds / (float)RealSecondsInADay;
                //how many seconds past according to game time.
                var secondsPastInGame = RealGameSecondsPast / ratio;

                var yearx = secondsPastInGame / RealSecondsInAYear;
                secondsPastInGame = secondsPastInGame % RealSecondsInAYear;

                var monthx = secondsPastInGame / RealSecondsInAMonth;
                secondsPastInGame = secondsPastInGame % RealSecondsInAMonth;

                // day
                var dayx = secondsPastInGame / RealSecondsInADay;
                secondsPastInGame = secondsPastInGame % RealSecondsInADay;

                var hourx = secondsPastInGame / RealSecondsInAnHour;
                secondsPastInGame %= RealSecondsInAnHour;

                var minutesx = secondsPastInGame / RealSecondsInAMinute;
                secondsPastInGame %= RealSecondsInAMinute;

                var secondsx = secondsPastInGame;

                if ((int)hourx != currentHour)
                {
                    currentHour = (int)hourx;
                    if (OnAfterHourChanged != null)
                    {
                        OnAfterHourChanged.Invoke();
                    }
                }

                if ((int)dayx != currentDay)
                {
                    currentDay = (int)dayx;
                    if (OnAfterDayChanged != null)
                    {
                        OnAfterDayChanged.Invoke();
                    }
                }

                if ((int)monthx != currentMonth)
                {
                    currentMonth = (int)monthx;
                    if (OnAfterMonthChanged != null)
                    {
                        OnAfterMonthChanged.Invoke();
                    }
                }

                if ((int)yearx != currentYear)
                {
                    currentYear = (int)yearx;
                    if (OnAfterYearChanged != null)
                    {
                        OnAfterYearChanged.Invoke();
                    }
                }

                currentYear = (int)yearx;
                currentMonth = (int)monthx;
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