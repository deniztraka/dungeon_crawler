using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class Dexterity : BaseMobileStat
    {

        public Dexterity() : base("Dexterity", 10, 100)
        {
        }
    }
}