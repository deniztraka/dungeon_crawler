using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Items.Models;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    public abstract class BaseEquipmentItemSO : BaseItemSO
    {
        public float Durability = 10;
        public RuntimeAnimatorController Animator;
        internal virtual void Equip()
        {
            Debug.Log("Equipping" + this.Name);
        }
    }
}