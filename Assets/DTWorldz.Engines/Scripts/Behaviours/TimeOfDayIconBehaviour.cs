using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DTWorldz.Engines
{
    public class TimeOfDayIconBehaviour : TimeOfDayUIBehaviour
    {
        float targetAngle = 360;
        float turnSpeed = 0.1f;

        void Start()
        {
            //turnSpeed = TimeOfTheDay.DayLengthInSeconds / 86400f;
        }

        public override void UpdateMe()
        {
            var currentGameTime = TimeOfTheDay.GetGameTime();
            targetAngle = (currentGameTime.Hours * 360) / 24;
        }

        void Update()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), Time.deltaTime);
        }
    }
}
