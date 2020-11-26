using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.Items.Equipments.Weapons
{
    public abstract class BaseWeapon : BaseEquipment
    {
        public float BaseMinDamage;
        public float BaseMaxDamage;
        public WeaponType WeaponType;
    }
}