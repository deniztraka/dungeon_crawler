using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "HealthRegenerator", menuName = "DTWorldz.Items/Items/Consumables/HealthRegenerators", order = 1)]
    public class HealthRegeneratorSO : BaseConsumableItemSO
    {
        internal override void Use()
        {
            Debug.Log(Name + " is consumed.");
        }
    }
}