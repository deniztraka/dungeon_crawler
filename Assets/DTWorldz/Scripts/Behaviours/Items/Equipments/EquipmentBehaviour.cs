using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using DTWorldz.ScriptableObjects.Items.Equipments;
using UnityEngine;
namespace DTWorldz.Behaviours.Items
{
    public class EquipmentBehaviour : BaseItemBehaviour
    {        
        public int MinStrength;
        public int MinDexterity;

        public override void SetCount(int count)
        {
            count = 1;
        }

        public override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);            
        }

        public override void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality)
        {
            base.SetModifiers(minStatCount, maxStatCount, statQuality);
            var equipmentTemplate = ItemTemplate as BaseEquipment;
            MinStrength = equipmentTemplate.MinStrength;
            MinDexterity = equipmentTemplate.MinDexterity;
        }

        public new EquipmentItemModel GetModel()
        {
            var equipmentItemModel = new EquipmentItemModel();
            equipmentItemModel.ItemTemplate = ItemTemplate;
            equipmentItemModel.StrengthModifier = StrengthModifier;
            equipmentItemModel.DexterityModifier = DexterityModifier;
            equipmentItemModel.StatQuality = StatQuality;

            equipmentItemModel.MinDexReq = MinDexterity;
            equipmentItemModel.MinStrReq = MinStrength;

            return equipmentItemModel;
        }

        internal void Map(EquipmentItemModel equipmentItemModel)
        {
            StrengthModifier = equipmentItemModel.StrengthModifier;
            DexterityModifier = equipmentItemModel.DexterityModifier;
            StatQuality = equipmentItemModel.StatQuality;

            MinDexterity = equipmentItemModel.MinDexReq;
            MinStrength = equipmentItemModel.MinStrReq;
        }
    }
}