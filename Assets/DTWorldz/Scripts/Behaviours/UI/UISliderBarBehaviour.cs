using System;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class UISliderBarBehaviour : MonoBehaviour
    {
        protected IHealth HealthBehaviour;
        public Slider Slider;
        public GameObject HealthObject;
        public Text MaxHealthText;
        public Text CurrentHealthValueText;        
        public virtual void Start()
        {
            Slider = GetComponent<Slider>();
            if (HealthObject != null)
            {
                HealthBehaviour = HealthObject.GetComponent(typeof(IHealth)) as IHealth;
            }
            if (HealthBehaviour != null)
            {
                HealthBehaviour.OnHealthChanged += new HealthChanged(ValueChanged);
                Init();
            }
        }

        protected virtual void ValueChanged(float currentVal, float maxVal)
        {
            Slider.value = currentVal;
            Slider.maxValue = maxVal;
            if (MaxHealthText)
            {
                MaxHealthText.text = "/ " + String.Format("{0:0}", maxVal);
            }
            if (CurrentHealthValueText)
            {
                CurrentHealthValueText.text = String.Format("{0:0}", currentVal);
            }

        }

        protected virtual void Init()
        {
            Slider.maxValue = HealthBehaviour.MaxHealth;
            Slider.value = HealthBehaviour.CurrentHealth;
            if (MaxHealthText)
            {
                MaxHealthText.text = "/ " + String.Format("{0:0}", HealthBehaviour.MaxHealth);
            }
            if (CurrentHealthValueText)
            {
                CurrentHealthValueText.text = String.Format("{0:0}", HealthBehaviour.CurrentHealth);
            }
        }
    }
}