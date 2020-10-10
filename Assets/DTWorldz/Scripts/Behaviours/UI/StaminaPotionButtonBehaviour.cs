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

        public GameObject EffectPrefab;

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
            if (EffectPrefab != null)
            {
                var effectObj = Instantiate(EffectPrefab, playerStam.transform.position, Quaternion.identity, playerStam.transform);
                // var particleSystem = effectObj.GetComponents<ParticleSystem>();
                Destroy(effectObj, 3f);

            }
            playerStam.CurrentHealth += 25;
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
            UpdateText();
        }
    }
}
