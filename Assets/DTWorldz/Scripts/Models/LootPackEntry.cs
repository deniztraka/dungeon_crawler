using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public class LootPackEntry
    {
        public int MaxCount = 1;
        public int MinCount = 1;
        public float Chance;
        public List<LootPackItem> Items;
    }
}