using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.DB
{
    [CreateAssetMenu(fileName = "ItemDataBase", menuName = "Database/ItemDataBase", order = 0)]
    public class ItemDatabase : ScriptableObject
    {
        public WeaponDatabase WeaponDataBase;
    }
}