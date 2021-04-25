using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "Consumable", menuName = "DTWorldz.Items/Consumable", order = 1)]
    public class ConsumableItemSO : BaseItemSO
    {
        internal override void Use()
        {
            Debug.Log(Name + " is consumed.");
        }
    }
}