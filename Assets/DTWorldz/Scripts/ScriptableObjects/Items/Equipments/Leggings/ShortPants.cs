using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.Items.Equipments.Weapons
{
    [CreateAssetMenu(fileName = "ShortPants", menuName = "Items/Equipments/Leggings/ShortPants", order = 0)]
    public class ShortPants : BaseEquipment
    {
        public ShortPants()
        {           
            Name = "Short Pants";
            MinStrength = 0;
            MinDexterity = 0;
        }
    }
}