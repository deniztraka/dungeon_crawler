using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.ScriptableObjects.Items
{
    public enum ItemType
    {
        Equipment = 0,
        Weapon = 1,
        Shield = 2,
        None = 3
    }

    [Serializable]
    public abstract class BaseItem : ScriptableObject
    {
        public GameObject Prefab;
        public string Name;
        public string Description;
        public Sprite Icon;

        public ItemType ItemType;
    }
}