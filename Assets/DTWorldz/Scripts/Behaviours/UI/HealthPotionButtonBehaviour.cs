using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Player;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class HealthPotionButtonBehaviour : ActionButtonBehaviour
    {
        public Text CountText;
        int count = 1;
        PlayerBehaviour player;
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
            player = playerGameObject.GetComponent<PlayerBehaviour>();
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
                var effectObj = Instantiate(EffectPrefab, player.transform.position, Quaternion.identity, player.transform);
                // var particleSystem = effectObj.GetComponents<ParticleSystem>();
                Destroy(effectObj, 3f);

            }
            player.DrinkHealthPotion();
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

