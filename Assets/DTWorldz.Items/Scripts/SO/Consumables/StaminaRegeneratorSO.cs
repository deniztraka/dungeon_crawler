using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using UnityEngine;

namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "StaminaRegenerator", menuName = "DTWorldz.Items/Items/Consumables/StaminaRegenerators", order = 1)]
    public class StaminaRegeneratorSO : BaseConsumableItemSO
    {
        internal override void Use()
        {
            base.Use();
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            var player = playerGameObject.GetComponent<PlayerBehaviour>();
            player.DrinkStaminaPotion(this.RegenAmount);
        }
    }
}