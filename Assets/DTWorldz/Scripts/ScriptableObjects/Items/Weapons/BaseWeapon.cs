using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.Items.Weapons
{
    public abstract class BaseWeapon : BaseItem
    {
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public float MinStrength;
        public float MinDexterity;
        public WeaponType Type;
    }
}