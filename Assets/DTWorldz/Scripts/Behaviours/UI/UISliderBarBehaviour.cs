using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class UISliderBarBehaviour : MonoBehaviour
    {        
        private IHealth healthBehaviour;
        private Slider slider;

        public GameObject HealthObject;

        void Start()
        {
            slider = GetComponent<Slider>();
            if(HealthObject != null){
                healthBehaviour = HealthObject.GetComponent(typeof(IHealth)) as IHealth;
            }
            if (healthBehaviour != null)
            {
                healthBehaviour.OnHealthChanged += new HealthChanged(ValueChanged);
                Init();
            }
        }

        void ValueChanged(float currentVal, float maxVal)
        {
            slider.value = currentVal;
            slider.maxValue = maxVal;
        }

        private void Init()
        {
            slider.maxValue = healthBehaviour.MaxHealth;
            slider.value = healthBehaviour.CurrentHealth;
        }
    }
}