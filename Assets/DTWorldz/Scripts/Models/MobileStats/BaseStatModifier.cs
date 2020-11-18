using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models.MobileStats
{
    [Serializable]
    public abstract class BaseStatModifier
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int statValue;
        public int Value
        {
            get { return statValue; }
            set { statValue = value; }
        }

        public BaseStatModifier(string name, int val)
        {
            this.name = name;
            this.statValue = val;
        }

        public virtual void AddModifier(BaseMobileStat stat)
        {
            stat.MaxValue += statValue;
        }

        public virtual void RemoveModifier(BaseMobileStat stat)
        {
            stat.MaxValue -= statValue;
        }
    }
}
