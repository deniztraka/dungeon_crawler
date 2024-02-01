using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using DTWorldz.Scripts.Managers;
using UnityEngine;

namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "StaminaRegenerator", menuName = "DTWorldz.Items/Items/Consumables/StaminaRegenerators", order = 1)]
    public class StaminaRegeneratorSO : BaseConsumableItemSO
    {
        internal override void Use()
        {
            base.Use();
            //GameManager.Instance.PlayerBehaviour.DrinkStaminaPotion(this.RegenAmount);
        }
    }
}