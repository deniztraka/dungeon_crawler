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

        public virtual void Start()
        {
            Slider = GetComponent<Slider>();
            if(HealthObject != null){
                HealthBehaviour = HealthObject.GetComponent(typeof(IHealth)) as IHealth;
            }
            if (HealthBehaviour != null)
            {
                HealthBehaviour.OnHealthChanged += new HealthChanged(ValueChanged);
                Init();
            }
        }

        protected void ValueChanged(float currentVal, float maxVal)
        {
            Slider.value = currentVal;
            Slider.maxValue = maxVal;
        }

        protected void Init()
        {
            Slider.maxValue = HealthBehaviour.MaxHealth;
            Slider.value = HealthBehaviour.CurrentHealth;
        }
    }
}