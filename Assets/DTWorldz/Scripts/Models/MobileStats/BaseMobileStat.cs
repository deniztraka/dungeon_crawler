using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Interfaces;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public class BaseMobileStat : IMobileStat
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int currentValue;
        public int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }
        private int maxValue;
        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        public BaseMobileStat(string name, int currentValue, int maxValue)
        {
            this.name = name;
            this.currentValue = currentValue;
            this.maxValue = maxValue;
        }
    }
}