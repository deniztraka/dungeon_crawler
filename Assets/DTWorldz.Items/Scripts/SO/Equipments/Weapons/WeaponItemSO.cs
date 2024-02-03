using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "DTWorldz.Items/Items/Weapon", order = 1)]
    public class WeaponItemSO : BaseEquipmentItemSO
    {
        public float DamageAddition = 10;
        public float AttackSpeedModifier = 1;
        public float RangeAddition = 1;
        public float CriticalChanceModifier = 0f;
        public float CriticalDamageModifier = 0f;
        public float KnockbackForceModifier = 0f;
    }
}