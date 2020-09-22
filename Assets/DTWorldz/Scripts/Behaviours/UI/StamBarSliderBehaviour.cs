using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class StamBarSliderBehaviour : UISliderBarBehaviour
    {
        public override void Start()
        {
            Slider = GetComponent<Slider>();
            if (HealthObject != null)
            {
                HealthBehaviour = HealthObject.GetComponent<StamBehaviour>();
            }
            if (HealthBehaviour != null)
            {
                HealthBehaviour.OnHealthChanged += new HealthChanged(ValueChanged);
                Init();
            }
        }
    }
}