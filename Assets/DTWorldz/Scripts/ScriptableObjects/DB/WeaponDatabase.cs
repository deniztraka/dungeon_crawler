using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.ScriptableObjects.DB
{
    [CreateAssetMenu(fileName = "WeaponDatabase", menuName = "Database/WeaponDatabase", order = 1)]
    public class WeaponDatabase : ScriptableObject
    {
        [SerializeField]
        private List<GameObject> weapons = new List<GameObject>();

        public UnityEngine.GameObject GetByName<GameObject>(string id)
        {
            return weapons.Find( w => w.name == name);            
        }
    }
}