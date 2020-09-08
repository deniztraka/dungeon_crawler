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
        public float Chance;
        public List<LootPackItem> Items;
    }
}