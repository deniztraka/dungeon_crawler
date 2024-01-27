using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    public abstract class BaseItemSO : ScriptableObject
    {
        public int Id;
        public string Name;
        public string Description;
        public Sprite Icon;
        public ItemType ItemType;
        public bool Stackable;

        public GameObject Prefab;

        public int MaxStackQuantity = 20;
    }
}