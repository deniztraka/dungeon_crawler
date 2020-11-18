using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models.MobileStats;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public class LootPackItem
    {
        public GameObject ItemPrefab;
        public int CountMin;
        public int CountMax;

        public int MinStatCount;
        public int MaxStatCount;
        public StatQuality StatQuality;        
    }
}