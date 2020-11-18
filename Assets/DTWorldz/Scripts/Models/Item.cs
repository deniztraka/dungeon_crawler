using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models.MobileStats;
using DTWorldz.ScriptableObjects.Items;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public class ItemModel
    {
        public StatQuality StatQuality;
        public BaseItem ItemTemplate;
        public DexterityModifier DexterityModifier;
        public StrengthModifier StrengthModifier;
        public int MinDamage;
        public int MaxDamage;        
    }
}