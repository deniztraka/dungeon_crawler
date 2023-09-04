using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DTWorldz.Behaviours.Items.Deco
{
    public class FireHoleTrapBehavior : TrapBehaviour
    {
        public UnityEngine.Rendering.Universal.Light2D Light;
        public override void Start()
        {
            base.Start();
            if (Light == null)
            {
                Light = transform.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
            }

            if (Light != null)
            {
                OnStateChanged += new TrapStateHandler(SetLights);
            }
        }

        private void SetLights(string newStateName)
        {
            if(newStateName.Equals("Damage")){
                Light.enabled = true;
            } else {
                Light.enabled = false;
            }
        }
    }
}