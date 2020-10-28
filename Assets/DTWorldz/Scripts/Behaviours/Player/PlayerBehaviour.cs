using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.UI;
using DTWorldz.DataModel;
using DTWorldz.SaveSystem;
using UnityEngine;
namespace DTWorldz.Behaviours.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private int goldAmount;        
        
        AudioManager audioManager;
        HealthBehaviour health;
        StamBehaviour stamina;
        public ActionButtonBehaviour ActionButtonBehaviour;
        public HealthPotionButtonBehaviour HealthPotionButtonBehaviour;
        public StaminaPotionButtonBehaviour StaminaPotionButtonBehaviour;
        public GameObject GoldLootPrefab;

        // Start is called before the first frame update
        void Start()
        {
            var movementBehaviour = GetComponent<PlayerMovementBehaviour>();
            var attackBehaviour = GetComponentInChildren<AttackBehaviour>();
            ActionButtonBehaviour.SetAction(movementBehaviour.Attack, attackBehaviour.AttackingFrequency);
            health = GetComponent<HealthBehaviour>();
            stamina = GetComponent<StamBehaviour>();
            audioManager = gameObject.GetComponent<AudioManager>();

            SaveSystemManager.Instance.OnGameSave += new SaveSystemManager.SaveSystemHandler(OnSave);
            SaveSystemManager.Instance.OnGameLoad += new SaveSystemManager.SaveSystemHandler(OnLoad);
        }

        internal void CollectGold(int count)
        {
            goldAmount += count;
            var isPlural = count > 1;
            if (GoldLootPrefab != null)
            {
                PopUpFloatingGold(count);
            }
        }

        internal void DrinkHealthPotion()
        {
            audioManager.Play("Drink");
            health.CurrentHealth += 20;
        }
        internal void DrinkStaminaPotion()
        {
            audioManager.Play("Drink");
            stamina.CurrentHealth += 30;
        }

        internal void CollectHealthPotion()
        {
            audioManager.Play("Loot");
            HealthPotionButtonBehaviour.AddPotion();
        }

        internal void CollectStaminaPotion()
        {
            audioManager.Play("Loot");
            StaminaPotionButtonBehaviour.AddPotion();
        }

        private void PopUpFloatingGold(float goldCount)
        {
            var randomXOffSet = UnityEngine.Random.Range(-0.1f, 0.2f);
            var newPos = new Vector3(transform.position.x + randomXOffSet, transform.position.y + 1.5f, transform.position.z);

            var floatingDamage = Instantiate(GoldLootPrefab, newPos, Quaternion.identity, transform);
            var floatingText = floatingDamage.GetComponent<FloatingGoldLootTextBehaviour>();
            floatingText.SetText(String.Format("{0:0}", goldCount));
        }

        private void OnLoad()
        {
            var playerDataModel = new PlayerDataModel();
            playerDataModel.Load();
            goldAmount = playerDataModel.GoldAmount;
        }

        private void OnSave()
        {            
            var playerDataModel = new PlayerDataModel();
            playerDataModel.GoldAmount = 10;
            playerDataModel.Save(); 
        }
    }
}
