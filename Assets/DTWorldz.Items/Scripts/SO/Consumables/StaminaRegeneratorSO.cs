using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "StaminaRegenerator", menuName = "DTWorldz.Items/Items/Consumables/StaminaRegenerators", order = 1)]
    public class StaminaRegeneratorSO : BaseConsumableItemSO
    {
        internal override void Use()
        {
            Debug.Log(Name + " is consumed.");
        }
    }
}