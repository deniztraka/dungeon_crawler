using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{


    public class StaminaPotionButtonBehaviour : ActionButtonBehaviour
    {
        public Text CountText;
        int count = 1;

        StamBehaviour playerStam;

        public override void Start()
        {
            base.Start();
            if (CountText == null)
            {
                CountText = gameObject.GetComponentInChildren<Text>();
            }
            SetAction(DrinkStamina, 3f);
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerStam = playerGameObject.GetComponent<StamBehaviour>();
        }

        public override void Update()
        {
            if (count <= 0)
            {
                return;
            }
            base.Update();
        }

        public void DrinkStamina()
        {
            playerStam.CurrentHealth += 10;
            count--;
            UpdateText();
        }

        private void UpdateText()
        {
            CountText.text = count.ToString();
        }

        public void AddPotion()
        {
            count++;
        }
    }
}
