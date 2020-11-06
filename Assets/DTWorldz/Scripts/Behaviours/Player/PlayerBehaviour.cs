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
        private int healthPotionAmount;
        private int stamPotionAmount;

        private SaveSystemManager saveSystemManager;
        private PlayerDataModel playerDataModel;

        AudioManager audioManager;
        HealthBehaviour health;
        StamBehaviour stamina;
        public ActionButtonBehaviour ActionButtonBehaviour;
        public HealthPotionButtonBehaviour HealthPotionButtonBehaviour;
        public StaminaPotionButtonBehaviour StaminaPotionButtonBehaviour;
        public GameObject GoldLootPrefab;

        public delegate void DataLoaderHandler();
        public event DataLoaderHandler OnAfterDataLoad;

        // Start is called before the first frame update
        void Start()
        {
            var movementBehaviour = GetComponent<PlayerMovementBehaviour>();
            var attackBehaviour = GetComponentInChildren<AttackBehaviour>();
            if (ActionButtonBehaviour)
            {
                ActionButtonBehaviour.SetAction(movementBehaviour.Attack, attackBehaviour.AttackingFrequency);
            }
            health = GetComponent<HealthBehaviour>();
            stamina = GetComponent<StamBehaviour>();
            audioManager = gameObject.GetComponent<AudioManager>();

            saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            if (saveSystemManager)
            {
                saveSystemManager.OnGameSave += new SaveSystemManager.SaveSystemHandler(Save);

                OnAfterDataLoad += new DataLoaderHandler(UpdateUI);
                Load();
            }
        }

        private void UpdateUI()
        {
            var characterBarCanvas = GameObject.FindObjectOfType<CharacterBarCanvas>();
            if (characterBarCanvas != null)
            {
                characterBarCanvas.UpdateCharacterTitleText(playerDataModel.Name);
            }
        }

        public void CreateDataModel()
        {
            playerDataModel = new PlayerDataModel(saveSystemManager);
        }

        internal void CollectGold(int count)
        {
            if (playerDataModel != null)
            {
                playerDataModel.GoldAmount += count;
            }
            var isPlural = count > 1;
            if (GoldLootPrefab != null)
            {
                PopUpFloatingGold(count);
            }
        }

        internal void DrinkHealthPotion()
        {
            if (playerDataModel != null)
            {
                playerDataModel.HealthPotionAmount--;
            }
            audioManager.Play("Drink");
            health.CurrentHealth += 20;
        }

        internal void DrinkStaminaPotion()
        {
            if (playerDataModel != null)
            {
                playerDataModel.StamPotionAmount--;
            }
            audioManager.Play("Drink");
            stamina.CurrentHealth += 30;
        }

        internal void CollectHealthPotion()
        {
            if (playerDataModel != null)
            {
                playerDataModel.HealthPotionAmount++;
            }
            audioManager.Play("Loot");
            HealthPotionButtonBehaviour.AddPotion();
        }

        internal void CollectStaminaPotion()
        {
            if (playerDataModel != null)
            {
                playerDataModel.StamPotionAmount++;
            }
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

        public bool Load()
        {
            if (saveSystemManager != null && saveSystemManager.HasSavedGame())
            {
                playerDataModel = new PlayerDataModel(saveSystemManager);
                playerDataModel.Load();

                if (StaminaPotionButtonBehaviour != null)
                {
                    for (int i = 0; i < playerDataModel.StamPotionAmount; i++)
                    {
                        StaminaPotionButtonBehaviour.AddPotion();
                    }
                }

                if (HealthPotionButtonBehaviour != null)
                {
                    for (int i = 0; i < playerDataModel.HealthPotionAmount; i++)
                    {
                        HealthPotionButtonBehaviour.AddPotion();
                    }
                }
                if (OnAfterDataLoad != null)
                {

                    OnAfterDataLoad();
                }

                return true;
            }
            return false;
        }

        public void Save()
        {
            if (saveSystemManager != null)
            {
                playerDataModel.Save();
            }
        }

        public PlayerDataModel GetDataModel()
        {
            return playerDataModel;
        }

        public void SetCharacterName(string name)
        {
            playerDataModel.Name = name;
        }
    }
}
