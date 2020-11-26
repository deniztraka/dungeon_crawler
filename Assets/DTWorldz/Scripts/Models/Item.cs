using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public class WeaponItemModel : EquipmentItemModel
    {
        public int MinDamage;
        public int MaxDamage;
    }

    [Serializable]
    public class EquipmentItemModel : ItemModel
    {
        public int MinStrReq;
        public int MinDexReq;
    }
    [Serializable]
    public class ItemModel
    {
        public StatQuality StatQuality;
        public BaseItem ItemTemplate;
        public DexterityModifier DexterityModifier;
        public StrengthModifier StrengthModifier;
    }
}