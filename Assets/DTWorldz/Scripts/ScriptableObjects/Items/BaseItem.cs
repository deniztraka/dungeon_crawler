using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using DTWorldz.Models.MobileStats;
using UnityEngine;
using UnityEngine.UI;

namespace DTWorldz.ScriptableObjects.Items
{
    [Serializable]
    public abstract class BaseItem : ScriptableObject
    {
        public GameObject Prefab;
        public string Name;
        public string Description;
        public Sprite Icon;
    }
}