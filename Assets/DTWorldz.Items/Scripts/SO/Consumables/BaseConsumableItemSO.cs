using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    public abstract class BaseConsumableItemSO : BaseItemSO
    {
        public float RegenAmount;
        internal virtual void Use()
        {
            Debug.Log(Name + " is consumed.");
        }
    }
}