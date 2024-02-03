using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Audios;
using DTWorldz.Behaviours.Items;
using DTWorldz.Behaviours.Mobiles;
using DTWorldz.Behaviours.UI;
using DTWorldz.Behaviours.Utils;
using DTWorldz.DataModel;
using DTWorldz.Items.Behaviours.UI;
using DTWorldz.Items.SO;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using DTWorldz.SaveSystem;
using DTWorldz.Scripts.Managers;
using UnityEngine;
namespace DTWorldz.Behaviours.Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        private SaveSystemManager saveSystemManager;



        private PlayerDataModel playerDataModel;

        AudioManager audioManager;
        HealthBehaviour health;
        StamBehaviour stamina;
        HungerBehaviour hunger;
        public float InteractionDistance = 1f;
        public ActionButtonBehaviour ActionButtonBehaviour;
        public ActionButtonBehaviour UseActionButtonBehavior;
        public HealthPotionButtonBehaviour HealthPotionButtonBehaviour;
        public StaminaPotionButtonBehaviour StaminaPotionButtonBehaviour;
        public GameObject GoldLootPrefab;
        private PlayerMovementBehaviour movementBehaviour;

        public Direction Direction { get { return movementBehaviour.GetDirection(); } }

        public delegate void DataLoaderHandler();
        public event DataLoaderHandler OnAfterDataLoad;

        // Start is called before the first frame update
        void Start()
        {
            movementBehaviour = GetComponent<PlayerMovementBehaviour>();
            var attackBehaviour = GetComponentInChildren<AttackBehaviour>();
            if (ActionButtonBehaviour)
            {
                ActionButtonBehaviour.SetAction(movementBehaviour.Attack, attackBehaviour.AttackingFrequency);
            }

            GetComponentInChildren<TargetFinder>().OnTargetChanged += new TargetFinder.TargetFinderEventHandler(OnInteractTargetChanged);

            health = GetComponent<HealthBehaviour>();
            stamina = GetComponent<StamBehaviour>();
            hunger = GetComponent<HungerBehaviour>();
            audioManager = gameObject.GetComponent<AudioManager>();

            var equipmentSlots = GameManager.Instance.PlayerEquipmentSlotsWrapper.GetComponentsInChildren<EquipmentSlotBehavior>();
            foreach (var slot in equipmentSlots)
            {
                slot.OnItemEquipped += new EquipmentSlotBehavior.ItemEquippedHandler(OnItemEquipped);
                slot.OnItemUnequipped += new EquipmentSlotBehavior.ItemEquippedHandler(OnItemUnequipped);
            }

            RegisterToSaveSystem();
        }

        private void OnItemUnequipped(BaseEquipmentItemSO itemSO)
        {
            if (itemSO is WeaponItemSO)
            {
                var weaponItemSO = itemSO as WeaponItemSO;
                var attackBehaviour = GetComponentInChildren<AttackBehaviour>();

                attackBehaviour.Damage -= weaponItemSO.DamageAddition;
                if (attackBehaviour.Damage < 0)
                {
                    attackBehaviour.Damage = 0;
                }
                if (weaponItemSO.AttackSpeedModifier > 0)
                {
                    attackBehaviour.AttackingFrequency /= weaponItemSO.AttackSpeedModifier;
                }
                if (attackBehaviour.AttackingFrequency < 0.1f)
                {
                    attackBehaviour.AttackingFrequency = 0.1f;
                }
                if (weaponItemSO.KnockbackForceModifier > 0)
                {
                    attackBehaviour.KnockbackForce /= weaponItemSO.KnockbackForceModifier;
                }
                attackBehaviour.AttackRange -= weaponItemSO.RangeAddition;
                if (attackBehaviour.AttackRange < 0)
                {
                    attackBehaviour.AttackRange = 0;
                }

                if(weaponItemSO.Animator != null){
                    GetComponent<PlayerMovementBehaviour>().AnimationSlots.Find(x => x.name == "Weapon").GetComponent<Animator>().runtimeAnimatorController = null;
                }
            }
        }

        private void OnItemEquipped(BaseEquipmentItemSO itemSO)
        {
            if (itemSO is WeaponItemSO)
            {
                var weaponItemSO = itemSO as WeaponItemSO;
                var attackBehaviour = GetComponentInChildren<AttackBehaviour>();


                attackBehaviour.Damage += weaponItemSO.DamageAddition;
                if (attackBehaviour.Damage < 0)
                {
                    attackBehaviour.Damage = 0;
                }
                if (weaponItemSO.AttackSpeedModifier > 0)
                {
                    attackBehaviour.AttackingFrequency *= weaponItemSO.AttackSpeedModifier;
                }
                if (attackBehaviour.AttackingFrequency < 0.1f)
                {
                    attackBehaviour.AttackingFrequency = 0.1f;
                }
                if (weaponItemSO.KnockbackForceModifier > 0)
                {
                    attackBehaviour.KnockbackForce *= weaponItemSO.KnockbackForceModifier;
                }
                attackBehaviour.AttackRange += weaponItemSO.RangeAddition;
                if (attackBehaviour.AttackRange < 0)
                {
                    attackBehaviour.AttackRange = 0;
                }

                if(weaponItemSO.Animator != null){
                    GetComponent<PlayerMovementBehaviour>().AnimationSlots.Find(x => x.name == "Weapon").GetComponent<Animator>().runtimeAnimatorController = weaponItemSO.Animator;
                }
            }
        }

        private void OnInteractTargetChanged(Interactable interactable)
        {
            if (UseActionButtonBehavior == null)
            {
                return;
            }

            if (interactable == null)
            {
                UseActionButtonBehavior.SetAction(null, 0);
                return;
            }

            UseActionButtonBehavior.SetAction(interactable.Interact, interactable.Cooldown, interactable.Image);
        }

        private void RegisterToSaveSystem()
        {
            saveSystemManager = GameObject.FindObjectOfType<SaveSystemManager>();
            if (saveSystemManager)
            {
                saveSystemManager.OnGameSave += new SaveSystemManager.SaveSystemHandler(Save);

                OnAfterDataLoad += new DataLoaderHandler(UpdateUI);
                Load();
            }
        }

        // internal bool PickupItem(BaseItemBehaviour itemBehaviour)
        // {
        //     audioManager.Play("Loot");
        //     return InventoryBehaviour.Instance.AddItem(itemBehaviour);
        // }

        private void UpdateUI()
        {
            var characterBarCanvas = GameObject.FindObjectOfType<CharacterBarCanvas>();
            if (characterBarCanvas != null)
            {
                characterBarCanvas.UpdateCharacterTitleText(playerDataModel.Name);
            }

            var characterPanelCanvasBehaviour = GameObject.FindObjectOfType<CharacterPanelCanvasBehaviour>();
            if (characterPanelCanvasBehaviour != null)
            {
                characterPanelCanvasBehaviour.UpdateGoldText(playerDataModel.GoldAmount);
                characterPanelCanvasBehaviour.UpdateStats(playerDataModel.Strength.CurrentValue, playerDataModel.Dexterity.CurrentValue);
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

            var characterPanelCanvasBehaviour = GameObject.FindObjectOfType<CharacterPanelCanvasBehaviour>();
            if (characterPanelCanvasBehaviour != null && playerDataModel != null)
            {
                characterPanelCanvasBehaviour.UpdateGoldText(playerDataModel.GoldAmount);
            }
        }

        internal void Eat(FoodItemSO foodItemSO)
        {
            audioManager.Play("Drink");
            hunger.Eat(foodItemSO);
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

                health.MaxHealth = playerDataModel.Strength.CurrentValue * 5;
                stamina.MaxHealth = playerDataModel.Dexterity.CurrentValue * 3;

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
