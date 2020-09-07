using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Models
{
    [System.Serializable]
    public class LootPackEntry
    {
        public float Chance { get; set; }
        // public ILootItem[] Items { get; set; }       
    }
}