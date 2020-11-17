using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Interfaces
{
    public interface IMobileStat
    {
        string Name { get; set; }
        int CurrentValue { get; set; }
        int MaxValue { get; set; }
    }
}