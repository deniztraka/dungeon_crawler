using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace DTWorldz.Behaviours.Items.Deco
{
    public class FireHoleTrapBehavior : TrapBehaviour
    {
        public Light2D Light;
        public override void Start()
        {
            base.Start();
            if (Light == null)
            {
                Light = transform.GetComponentInChildren<Light2D>();
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