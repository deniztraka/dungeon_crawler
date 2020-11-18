using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class StrengthModifier : BaseStatModifier
    {
        public StrengthModifier(int val) : base("Strength", val)
        {
        }
    }
}