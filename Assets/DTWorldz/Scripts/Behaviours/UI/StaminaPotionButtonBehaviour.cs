using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Player;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class StaminaPotionButtonBehaviour : ActionButtonBehaviour
    {
        public Text CountText;
        int count;

        public GameObject EffectPrefab;

        PlayerBehaviour player;

        public override void Start()
        {
            base.Start();
            if (CountText == null)
            {
                CountText = gameObject.GetComponentInChildren<Text>();
            }

            SetAction(DrinkStamina, 2f);
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            player = playerGameObject.GetComponent<PlayerBehaviour>();
            UpdateText();
            SetFillAmount(count > 0 ? 1f : 0f);
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
            if (count > 0)
            {
                if (EffectPrefab != null)
                {
                    var effectObj = Instantiate(EffectPrefab, player.transform.position, Quaternion.identity, player.transform);
                    // var particleSystem = effectObj.GetComponents<ParticleSystem>();
                    Destroy(effectObj, 2f);

                }
                player.DrinkStaminaPotion();

                count--;
                UpdateText();
            }
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
