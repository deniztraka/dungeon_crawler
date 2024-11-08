﻿using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.Player;
using DTWorldz.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.Behaviours.UI
{
    public class HealthPotionButtonBehaviour : ActionButtonBehaviour
    {
        public Text CountText;
        int count;
        PlayerBehaviour player;
        public GameObject EffectPrefab;
        public override void Start()
        {
            base.Start();
            if (CountText == null)
            {
                CountText = gameObject.GetComponentInChildren<Text>();
            }
            SetAction(DrinkHealth, 2f);
            player = GameManager.Instance.PlayerBehaviour;
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

        public void DrinkHealth()
        {
            if (count > 0)
            {
                if (EffectPrefab != null)
                {
                    var effectObj = Instantiate(EffectPrefab, player.transform.position, Quaternion.identity, player.transform);
                    // var particleSystem = effectObj.GetComponents<ParticleSystem>();
                    Destroy(effectObj, 2f);

                }
                //player.DrinkHealthPotion();
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

