using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public class LootPackItem
    {
        public GameObject ItemPrefab;
        public int CountMin;
        public int CountMax;
    }
}