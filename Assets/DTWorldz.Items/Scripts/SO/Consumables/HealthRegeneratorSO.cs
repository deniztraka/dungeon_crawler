using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Player;
using UnityEngine;
namespace DTWorldz.Items.SO
{
    [CreateAssetMenu(fileName = "HealthRegenerator", menuName = "DTWorldz.Items/Items/Consumables/HealthRegenerators", order = 1)]
    public class HealthRegeneratorSO : BaseConsumableItemSO
    {
        
        internal override void Use()
        {
            base.Use();
            var playerGameObject = GameObject.FindGameObjectWithTag("Player");
            var player = playerGameObject.GetComponent<PlayerBehaviour>();
            player.DrinkHealthPotion(this.RegenAmount);
            
        }
    }
}