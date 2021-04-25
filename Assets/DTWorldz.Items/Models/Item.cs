using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.Models
{
    public enum ItemType
    {
        None = 0,
        Consumable = 1,
        Equipment = 2,
        Weapon = 3
    }
    
    [Serializable]
    public class Item
    {
        public int Id;
        public string Name;
        public string Description;
        public Sprite Icon;
        public ItemType ItemType;
    }
}