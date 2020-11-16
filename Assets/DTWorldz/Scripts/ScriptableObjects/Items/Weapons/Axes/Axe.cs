using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.Items.Weapons
{
    [CreateAssetMenu(fileName = "Axe", menuName = "Items/Axes/Axe", order = 0)]
    public class Axe : BaseWeapon
    {
        public Axe()
        {
            Type = Models.WeaponType.Axe;
            Name = "War Axe";
            MinStrength = 10;
            MinDexterity = 10;
            BaseMinDamage = 5;
            BaseMaxDamage = 8;
        }
    }
}