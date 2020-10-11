using System;
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

        protected override void ValueChanged(float currentVal, float maxVal)
        {
            Slider.value = currentVal;
            Slider.maxValue = maxVal;
            if (MaxHealthText)
            {
                MaxHealthText.text = String.Format("{0:0}", maxVal) + " \\";
            }
            if (CurrentHealthValueText)
            {
                CurrentHealthValueText.text = String.Format("{0:0}", currentVal);
            }

        }

        protected override void Init()
        {
            Slider.maxValue = HealthBehaviour.MaxHealth;
            Slider.value = HealthBehaviour.CurrentHealth;
            if (MaxHealthText)
            {
                MaxHealthText.text = String.Format("{0:0}", HealthBehaviour.MaxHealth) + " \\";
            }
            if (CurrentHealthValueText)
            {
                CurrentHealthValueText.text = String.Format("{0:0}", HealthBehaviour.CurrentHealth);
            }
        }
    }
}