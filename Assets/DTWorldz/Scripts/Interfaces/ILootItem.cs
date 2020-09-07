using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Interfaces
{
    public interface ILootItem
    {
        int Count { get; }
        void SetCount(int count);
        bool IsStackable { get; set; }
        void OnAfterInstantiation();
    }
}