using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models.MobileStats;
using UnityEngine;
namespace DTWorldz.Interfaces
{
    public interface ILootItem
    {
        int Count { get; }
        void SetCount(int count);
        bool IsStackable { get; }
        void OnAfterDrop();
        void SetModifiers(int minStatCount, int maxStatCount, StatQuality statQuality);
    }
}