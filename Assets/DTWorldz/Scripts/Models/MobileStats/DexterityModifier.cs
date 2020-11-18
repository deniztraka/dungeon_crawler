using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class DexterityModifier : BaseStatModifier
    {
        public DexterityModifier(int val) : base("Dexterity", val)
        {
        }
    }
}