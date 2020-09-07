using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEngine;
namespace DTWorldz.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemDropTemplate", menuName = "ScriptableObjects/ItemDropTemplate", order = 3)]
    public class ItemDropTemplate : ScriptableObject
    {
        public List<LootPackEntry> Entriess;
    }
}