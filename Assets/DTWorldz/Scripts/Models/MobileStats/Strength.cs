using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class Strength : BaseMobileStat
    {
        public Strength() : base("Strength", 10, 100)
        {
        }
    }
}