using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Interfaces;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using UnityEngine;
namespace DTWorldz.Behaviours.Items
{
    public class WeaponBehaviour : EquipmentBehaviour
    {
        public int MinDamage;
        public int MaxDamage;
        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.tag == "Player")
            {

            }
        }

        public override void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality)
        {
            base.SetModifiers(minStatCount, maxStatCount, statQuality);
            var minDamageAddition = 0;
            var maxDamageAddition = 0;

            // set min-max damage values
            switch (StatQuality)
            {
                case StatQuality.Poor:
                    minDamageAddition = 0;
                    maxDamageAddition = 0;
                    break;
                case StatQuality.Regular:
                    minDamageAddition = 2;
                    maxDamageAddition = 6;
                    break;
                case StatQuality.Exceptional:
                    minDamageAddition = 6;
                    maxDamageAddition = 10;
                    break;
                case StatQuality.Rare:
                    minDamageAddition = 10;
                    maxDamageAddition = 15;
                    break;
                case StatQuality.Legendary:
                    minDamageAddition = 15;
                    maxDamageAddition = 20;
                    break;
            }

            if (minDamageAddition != 0)
            {
                MinDamage += (int)Mathf.Floor(Random.Range(MinDamage, MinDamage + minDamageAddition));
            }
            if (maxDamageAddition != 0)
            {
                MaxDamage += (int)Mathf.Floor(Random.Range(MaxDamage, MaxDamage + maxDamageAddition));
            }
        }

        public new WeaponItemModel GetModel()
        {
            var weaponItemModel = new WeaponItemModel();
            weaponItemModel.ItemTemplate = ItemTemplate;
            weaponItemModel.StrengthModifier = StrengthModifier;
            weaponItemModel.DexterityModifier = DexterityModifier;
            weaponItemModel.StatQuality = StatQuality;

            weaponItemModel.MinDamage = MinDamage;
            weaponItemModel.MaxDamage = MaxDamage;

            weaponItemModel.MinDexReq = MinDexterity;
            weaponItemModel.MinStrReq = MinStrength;

            return weaponItemModel;
        }

        internal void Map(WeaponItemModel weaponItemModel)
        {
            StrengthModifier = weaponItemModel.StrengthModifier;
            DexterityModifier = weaponItemModel.DexterityModifier;
            StatQuality = weaponItemModel.StatQuality;

            MinDamage = weaponItemModel.MinDamage;
            MaxDamage = weaponItemModel.MaxDamage;

            MinDexterity = weaponItemModel.MinDexReq;
            MinStrength = weaponItemModel.MinStrReq;
        }
    }
}