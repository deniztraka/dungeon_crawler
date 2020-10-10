using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class HealthPotionButtonBehaviour : ActionButtonBehaviour
    {
        public Text CountText;
        int count = 1;
        HealthBehaviour playerHealth;
        public GameObject EffectPrefab;
        public override void Start()
        {
            base.Start();
            if (CountText == null)
            {
                CountText = gameObject.GetComponentInChildren<Text>();
            }
            SetAction(DrinkHealth, 3f);
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            playerHealth = playerGameObject.GetComponent<HealthBehaviour>();
            UpdateText();
        }

        public override void Update()
        {
            if (count <= 0)
            {
                return;
            }
            base.Update();
        }

        public void DrinkHealth()
        {
            if (EffectPrefab != null)
            {
                var effectObj = Instantiate(EffectPrefab, playerHealth.transform.position, Quaternion.identity, playerHealth.transform);
                // var particleSystem = effectObj.GetComponents<ParticleSystem>();
                Destroy(effectObj, 3f);

            }
            playerHealth.CurrentHealth += 20;
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

