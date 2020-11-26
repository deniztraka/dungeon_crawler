using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.Items.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "Axe", menuName = "Items/Axes/Axe", order = 0)]
    public class Axe : BaseWeapon
    {
        public Axe()
        {
            WeaponType = Models.WeaponType.Axe;
            Name = "War Axe";
            MinStrength = 10;
            MinDexterity = 10;
            BaseMinDamage = 5;
            BaseMaxDamage = 8;
        }
    }
}